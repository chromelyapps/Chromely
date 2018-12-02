// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpHttpSchemeHandler.cs" company="Chromely Projects">
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
    using System.Text;
    using System.Threading.Tasks;
    using Chromely.CefSharp.Winapi.RestfulService;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using global::CefSharp;

    /// <summary>
    /// The CefSharp http scheme handler.
    /// </summary>
    public class CefSharpHttpSchemeHandler : ResourceHandler
    {
        /// <summary>
        /// The ChromelyResponse object.
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
            bool isCustomScheme = UrlSchemeProvider.IsUrlOfRegisteredCustomScheme(request.Url);
            if (isCustomScheme)
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            this.mChromelyResponse = RequestTaskRunner.Run(request);

                            string jsonData = this.mChromelyResponse.Data.EnsureResponseIsJsonFormat();
                            var content = Encoding.UTF8.GetBytes(jsonData);
                            this.mStream.Write(content, 0, content.Length);
                            this.mMimeType = "application/json";
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);

                            this.mChromelyResponse =
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
            responseLength = this.mStream.Length;
            redirectUrl = null;

            var headers = response.ResponseHeaders;
            headers.Add("Cache-Control", "private");
            headers.Add("Access-Control-Allow-Origin", "*");
            headers.Add("Access-Control-Allow-Methods", "GET,POST");
            headers.Add("Access-Control-Allow-Headers", "Content-Type");
            headers.Add("Content-Type", "application/json; charset=utf-8");
            response.ResponseHeaders = headers;

            response.StatusCode = this.mChromelyResponse.Status;
            response.StatusText = this.mChromelyResponse.StatusText;
            response.MimeType = this.mMimeType; 

            return this.mStream;
        }
    }
}
