// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpResourceSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable StyleCop.SA1210
namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using global::CefSharp;

    /// <summary>
    /// The CefSharp resource scheme handler.
    /// </summary>
    public class CefSharpResourceSchemeHandler : ResourceHandler
    {
        /// <summary>
        /// The ChromelyResponse response.
        /// </summary>
        private ChromelyResponse mChromelyResponse;

        /// <summary>
        /// The mime type.
        /// </summary>
        private string mMimeType;

        /// <summary>
        /// The stream object.
        /// </summary>
        private Stream mStream = new MemoryStream();

        /// <summary>
        /// The process request async.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var u = new Uri(request.Url);
            var file = u.Authority + u.AbsolutePath;

            if (File.Exists(file))
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            mChromelyResponse = new ChromelyResponse { Status = (int)HttpStatusCode.OK, Data = "OK." };

                            byte[] fileBytes = File.ReadAllBytes(file);
                            mStream = new MemoryStream(fileBytes);

                            string extension = Path.GetExtension(file);
                            mMimeType = MimeMapper.GetMimeType(extension);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);

                            mChromelyResponse =
                                new ChromelyResponse
                                    {
                                        Status = (int)HttpStatusCode.BadRequest,
                                        Data = "An error occured."
                                    };
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });

                return true;
            }

            Log.Error($"File {request.Url} is not valid.");
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// The get response.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="responseLength">
        /// The response length.
        /// </param>
        /// <param name="redirectUrl">
        /// The redirect url.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public override Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = mStream.Length;
            redirectUrl = null;

            response.StatusCode = mChromelyResponse.Status;
            response.StatusText = mChromelyResponse.StatusText;
            response.MimeType = mMimeType;

            return mStream;
        }
    }
}
