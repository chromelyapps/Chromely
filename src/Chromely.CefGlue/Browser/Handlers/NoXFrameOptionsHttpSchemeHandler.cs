// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoXFrameOptionsHttpSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Infrastructure;
using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// Custom http requests handler. Acts as proxy for CEF browser HTTP requests. 
    /// Omits X-Frame-Options header from response
    /// </summary>
    public class NoXFrameOptionsHttpSchemeHandler : CefResourceHandler, IDisposable
    {
        const int _bufferSize = 8 * 1024;
        readonly HttpClient _httpClient;
        HttpResponseMessage _httpResponseMessage;
        CancellationTokenSource _cancellationTokenSource;
        Stream _contentStream;
        byte[] _rentedBuffer;
        int _bytesRead = -1;

        /// <summary>
        /// Initializes a new instance of the Chromely.CefGlue.Browser.Handlers.NoXFrameOptionsHttpSchemeHandler class.
        /// </summary>
        /// <param name="httpClient">Object will send requests</param>        
        public NoXFrameOptionsHttpSchemeHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _httpResponseMessage?.Dispose();
            _cancellationTokenSource?.Dispose();

            _httpResponseMessage = null;
            _cancellationTokenSource = null;
            _contentStream = null; //Is this safe?
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~NoXFrameOptionsHttpSchemeHandler() //DO we really need this?
        {
            FreeBuffer();
        }

        /// <inheritdoc/>
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var httpRequest = BuildHttpRequest(request);
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {

                try
                {
                    _httpResponseMessage = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    Log.Warn("Cancellation requested");
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
                finally
                {
                    callback.Continue();
                    callback.Dispose();
                }
            }, _cancellationTokenSource.Token);

            return true;
        }

        /// <inheritdoc/>
        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            // unknown content-length
            // no-redirect
            responseLength = -1;
            redirectUrl = null; 

            try
            {
                var headers = response.GetHeaderMap();
                headers.Clear();

                foreach (var header in _httpResponseMessage.Headers)
                {
                    if (header.Key == "X-Frame-Options")
                        continue;
                    foreach (var val in header.Value)
                        headers.Add(header.Key, val);
                }
                
                if (_httpResponseMessage.Content != null)
                {
                    foreach (var header in _httpResponseMessage.Content.Headers)
                    {
                        foreach (var val in header.Value)
                            headers.Add(header.Key, val);
                    }
                }
                headers["Access-Control-Allow-Origin"] = "*";
                response.SetHeaderMap(headers);

                response.MimeType = _httpResponseMessage.Content?.Headers?.ContentType?.MediaType;
                response.Status = (int)_httpResponseMessage.StatusCode;
                response.StatusText = _httpResponseMessage.ReasonPhrase;
                responseLength = _httpResponseMessage.Content?.Headers?.ContentLength ?? -1;

                if (_httpResponseMessage.StatusCode == HttpStatusCode.MovedPermanently
                    || _httpResponseMessage.StatusCode == HttpStatusCode.Moved
                    || _httpResponseMessage.StatusCode == HttpStatusCode.Redirect
                    || _httpResponseMessage.StatusCode == HttpStatusCode.RedirectMethod
                    || _httpResponseMessage.StatusCode == HttpStatusCode.TemporaryRedirect)
                    redirectUrl = _httpResponseMessage.Headers.Location.ToString();

            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.BadRequest;
                response.MimeType = "text/plain";
                response.StatusText = "Resource loading error.";

                Log.Error(exception);
            }
        }

        /// <inheritdoc/>
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            if (_bytesRead != -1)
            {
                if (_bytesRead != 0)
                    response.Write(_rentedBuffer, 0, _bytesRead);
                FreeBuffer();
                bytesRead = _bytesRead;
                _bytesRead = -1;
                return bytesRead != 0;
            }
            bytesRead = 0;
            Task.Run(async () =>
            {
                try
                {
                    _rentedBuffer = ArrayPool<byte>.Shared.Rent(_bufferSize);
                    if (_contentStream == null)
                        _contentStream = await _httpResponseMessage.Content.ReadAsStreamAsync();

                    _bytesRead = await _contentStream.ReadAsync(_rentedBuffer, 0, Math.Min(bytesToRead, _bufferSize), _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    FreeBuffer();
                    _bytesRead = 0;
                    Log.Warn("Cancellation requested");
                }
                catch (Exception exception)
                {
                    FreeBuffer();
                    _bytesRead = 0;

                    Log.Error(exception);
                }
                finally
                {
                    callback.Continue();
                    callback.Dispose();
                }
            }, _cancellationTokenSource.Token); //TODO: add cancellation
            return true;
        }
        
        /// <inheritdoc/>
        protected override bool CanGetCookie(CefCookie cookie)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override bool CanSetCookie(CefCookie cookie)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        void FreeBuffer()
        {
            if (_rentedBuffer != null)
            {
                ArrayPool<byte>.Shared.Return(_rentedBuffer);
                _rentedBuffer = null;
            }
        }

        static HttpMethod GetHttpMethod(string methodName)
        {
            switch (methodName.ToUpper())
            {
                case "GET":
                    return HttpMethod.Get;
                case "PUT":
                    return HttpMethod.Put;
                case "POST":
                    return HttpMethod.Post;
                case "DELETE":
                    return HttpMethod.Delete;
                case "HEAD":
                    return HttpMethod.Head;
                case "OPTIONS":
                    return HttpMethod.Options;
                case "TRACE":
                    return HttpMethod.Trace;
            }

            throw new ArgumentException($"Unknown http method: {methodName}");
        }

        static HttpRequestMessage BuildHttpRequest(CefRequest cefRequest)
        {
            var httpRequest = new HttpRequestMessage(GetHttpMethod(cefRequest.Method), cefRequest.Url);
            var cefHeaders = cefRequest.GetHeaderMap();
            foreach (var key in cefHeaders.AllKeys)
                httpRequest.Headers.TryAddWithoutValidation(key, cefHeaders.GetValues(key));

            if (cefRequest.PostData != null && cefRequest.PostData.Count > 0)
                httpRequest.Content = new StreamContent(new CefPostDataStream(cefRequest.PostData.GetElements()));

            return httpRequest;
        }
    }
}
