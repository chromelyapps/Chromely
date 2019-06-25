// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpResourceSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using global::CefSharp;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    /// <summary>
    /// The CefSharp resource scheme handler.
    /// </summary>
    public class CefSharpResourceSchemeHandler : ResourceHandler
    {
        /// <summary>
        /// The ChromelyResponse response.
        /// </summary>
        private ChromelyResponse _chromelyResponse;

        /// <summary>
        /// The mime type.
        /// </summary>
        private string _mimeType;

        /// <summary>
        /// The stream object.
        /// </summary>
        private Stream _stream = new MemoryStream();

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
                            _chromelyResponse = new ChromelyResponse { Status = (int)HttpStatusCode.OK, Data = "OK." };

                            byte[] fileBytes = File.ReadAllBytes(file);
                            _stream = new MemoryStream(fileBytes);

                            string extension = Path.GetExtension(file);
                            _mimeType = MimeMapper.GetMimeType(extension);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);

                            _chromelyResponse =
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
            responseLength = _stream.Length;
            redirectUrl = null;

            response.StatusCode = _chromelyResponse.Status;
            response.StatusText = _chromelyResponse.StatusText;
            response.MimeType = _mimeType;

            return _stream;
        }
    }
}
