// Copyright © 2014 The CefSharp Authors. All rights reserved.
// https://github.com/cefsharp/CefSharp/blob/master/CefSharp/ResourceHandler.cs
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    /// <summary>
    /// Default implementation of <see cref="CefResourceHandler"/>. This latest implementation provides some simplification, at
    /// a minimum you only need to override ProcessRequestAsync. See the project source on GitHub for working examples.
    /// used to implement a custom request handler interface. The methods of this class will always be called on the IO thread. 
    /// Static helper methods are included like FromStream and FromString that make dealing with fixed resources easy.
    /// </summary>
    public class ResourceHandler : CefResourceHandler
    {
        /// <summary>
        /// MimeType to be used if none provided
        /// </summary>
        public const string DefaultMimeType = "text/html";

        /// <summary>
        /// We reuse a temp buffer where possible for copying the data from the stream
        /// into the output stream
        /// </summary>
        private byte[] tempBuffer;

        /// <summary>
        /// Gets or sets the Charset
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the Mime Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the resource stream.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the http status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets ResponseLength, when you know the size of your
        /// Stream (Response) set this property. This is optional.
        /// If you use a MemoryStream and don't provide a value
        /// here then it will be cast and its size used
        /// </summary>
        public long? ResponseLength { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers { get; private set; }

        /// <summary>
        /// When true the Stream will be Disposed when
        /// this instance is Disposed. The default value for
        /// this property is false.
        /// </summary>
        public bool AutoDisposeStream { get; set; }

        /// <summary>
        /// If the ErrorCode is set then the response will be ignored and
        /// the errorCode returned.
        /// </summary>
        public CefErrorCode? ErrorCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandler"/> class.
        /// </summary>
        /// <param name="mimeType">Optional mimeType defaults to <see cref="DefaultMimeType"/></param>
        /// <param name="stream">Optional Stream - must be set at some point to provide a valid response</param>
        /// <param name="autoDisposeStream">When true the Stream will be disposed when this instance is Disposed, you will
        /// be unable to use this ResourceHandler after the Stream has been disposed</param>
        /// <param name="charset">response charset</param>
        public ResourceHandler(string mimeType = DefaultMimeType, Stream stream = null, bool autoDisposeStream = false, string charset = null)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException("mimeType", "Please provide a valid mimeType");
            }

            StatusCode = 200;
            StatusText = "OK";
            MimeType = mimeType;
            Headers = new NameValueCollection();
            Stream = stream;
            AutoDisposeStream = autoDisposeStream;
            Charset = charset;

            //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin
            //Potential workaround for requests coming from different scheme
            //e.g. request from https made to myScheme
            Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Begin processing the request. If you have the data in memory you can execute the callback
        /// immediately and return true. For Async processing you would typically spawn a Task to perform processing,
        /// then return true. When the processing is complete execute callback.Continue(); In your processing Task, simply set
        /// the StatusCode, StatusText, MimeType, ResponseLength and Stream
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>To handle the request return true and call
        /// <see cref="CefCallback.Continue"/> once the response header information is available
        /// <see cref="CefCallback.Continue"/> can also be called from inside this method if
        /// header information is available immediately).
        /// To cancel the request return false.</returns>
        public virtual CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
        {
            return CefReturnValue.Continue;
        }

        /// <summary>
        /// Gets a byteArray from the given string using the provided encoding
        /// </summary>
        /// <param name="text">string to be converted to a stream</param>
        /// <param name="encoding">encoding</param>
        /// <param name="includePreamble">if true a BOM will be written to the beginning of the stream</param>
        /// <returns>A memory stream from the given string</returns>
        public static byte[] GetByteArray(string text, Encoding encoding, bool includePreamble = true)
        {
            if (includePreamble)
            {
                var preamble = encoding.GetPreamble();
                var bytes = encoding.GetBytes(text);

                var memoryStream = new MemoryStream(preamble.Length + bytes.Length);

                memoryStream.Write(preamble, 0, preamble.Length);
                memoryStream.Write(bytes, 0, bytes.Length);

                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }

            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Dispose of resources here
        /// </summary>
        public virtual void Dispose()
        {
            if (AutoDisposeStream && Stream != null)
            {
                Stream.Dispose();
            }

            Stream = null;
            tempBuffer = null;
        }

        protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
        {
            var processRequest = ProcessRequestAsync(request, callback);

            //Process the request in an async fashion
            if (processRequest == CefReturnValue.ContinueAsync)
            {
                handleRequest = false;

                return true;
            }
            else if (processRequest == CefReturnValue.Continue)
            {
                handleRequest = true;

                return true;
            }

            //Cancel Request
            handleRequest = true;

            return false;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            redirectUrl = null;
            responseLength = -1;

            response.MimeType = MimeType;
            response.Status = StatusCode;
            response.StatusText = StatusText;
            response.SetHeaderMap(Headers);

            if (!string.IsNullOrEmpty(Charset))
            {
                response.Charset = Charset;
            }

            if (ResponseLength.HasValue)
            {
                responseLength = ResponseLength.Value;
            }

            if (Stream != null && Stream.CanSeek)
            {
                //ResponseLength property has higher precedence over Stream.Length
                if (ResponseLength == null || responseLength == 0)
                {
                    //If no ResponseLength provided then attempt to infer the length
                    responseLength = Stream.Length;
                }

                Stream.Position = 0;
            };
        }

        protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
        {
            //No Stream or Stream cannot seek then we indicate failure
            if (Stream == null || !Stream.CanSeek)
            {
                //Indicate failure
                bytesSkipped = -2;

                return false;
            }

            bytesSkipped = bytesToSkip;

            Stream.Seek(bytesToSkip, SeekOrigin.Current);

            //If data is available immediately set bytesSkipped to the number of of bytes skipped and return true.
            return true;
        }

        protected override bool Read(IntPtr dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
        {
            bytesRead = 0;

            //We don't need the callback, as it's an unmanaged resource we should dispose it (could wrap it in a using statement).
            callback.Dispose();

            if (Stream == null)
            {
                return false;
            }

            //Data out represents an underlying unmanaged buffer (typically 64kb in size).
            //We reuse a temp buffer where possible
            if (tempBuffer == null || tempBuffer.Length < bytesToRead)
            {
                tempBuffer = new byte[bytesToRead];
            }

            //Only read the number of bytes that can be written to dataOut
            bytesRead = Stream.Read(tempBuffer, 0, bytesToRead);

            // To indicate response completion set bytesRead to 0 and return false
            if (bytesRead == 0)
            {
                return false;
            }

            using (var safeBuffer = new CefSafeBuffer(dataOut, (ulong)bytesRead))
            using (var dataOutStream = new UnmanagedMemoryStream(safeBuffer, 0, bytesRead, FileAccess.Write))
            {
                //We need to use bytesRead instead of tempbuffer.Length otherwise
                //garbage from the previous copy would be written to dataOut
                dataOutStream.Write(tempBuffer, 0, bytesRead);
            }

            return bytesRead > 0;
        }

        protected override void Cancel()
        {
            // Prior to Prior to https://bitbucket.org/chromiumembedded/cef/commits/90301bdb7fd0b32137c221f38e8785b3a8ad8aa4
            // This method was unexpectedly being called during Read (from a different thread),
            // changes to the threading model were made and I haven't confirmed if this is still
            // the case.
            // 
            // The documentation for Cancel is vague and there aren't any examples that
            // illustrate its intended use case so for now we'll just keep things
            // simple and free our resources in Dispose
        }
    }
}