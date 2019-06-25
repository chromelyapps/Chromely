// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpHttpSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using global::CefSharp;
using Chromely.CefSharp.Winapi.RestfulService;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    /// <summary>
    /// The CefSharp http scheme handler.
    /// </summary>
    public class CefSharpHttpSchemeHandler : ResourceHandler
    {
        /// <summary>
        /// The ChromelyResponse object.
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
            bool isCustomScheme = UrlSchemeProvider.IsUrlOfRegisteredCustomScheme(request.Url);
            if (isCustomScheme)
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            _chromelyResponse = RequestTaskRunner.Run(request);

                            string jsonData = _chromelyResponse.Data.EnsureResponseIsJsonFormat();
                            var content = Encoding.UTF8.GetBytes(jsonData);
                            _stream.Write(content, 0, content.Length);
                            _mimeType = "application/json";
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

            Log.Error($"Url {request.Url} is not of a registered custom scheme.");
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

            var headers = response.Headers;
            headers.Add("Cache-Control", "private");
            headers.Add("Access-Control-Allow-Origin", "*");
            headers.Add("Access-Control-Allow-Methods", "GET,POST");
            headers.Add("Access-Control-Allow-Headers", "Content-Type");
            headers.Add("Content-Type", "application/json; charset=utf-8");
            response.Headers = headers;

            response.StatusCode = _chromelyResponse.Status;
            response.StatusText = _chromelyResponse.StatusText;
            response.MimeType = _mimeType; 

            return _stream;
        }
    }
}
