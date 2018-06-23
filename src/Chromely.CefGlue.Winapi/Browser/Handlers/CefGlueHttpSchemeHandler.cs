// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueHttpSchemeHandler.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Chromely.CefGlue.Winapi.RestfulService;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue http scheme handler.
    /// </summary>
    public class CefGlueHttpSchemeHandler : CefResourceHandler
    {
        /// <summary>
        /// The ChromelyResponse object.
        /// </summary>
        private ChromelyResponse mChromelyResponse;

        /// <summary>
        /// The response in bytes.
        /// </summary>
        private byte[] mResponseBytes;

        /// <summary>
        /// The completed flag.
        /// </summary>
        private bool mCompleted;

        /// <summary>
        /// The total bytes read.
        /// </summary>
        private int mTotalBytesRead;

        /// <summary>
        /// The process request.
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
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
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
                            this.mResponseBytes = Encoding.UTF8.GetBytes(jsonData);
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
        /// The get response headers.
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
        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            // unknown content-length
            // no-redirect
            responseLength = -1;
            redirectUrl = null;

            try
            {
                HttpStatusCode status = (this.mChromelyResponse != null) ? (HttpStatusCode)this.mChromelyResponse.Status : HttpStatusCode.BadRequest;
                string errorStatus = (this.mChromelyResponse != null) ? this.mChromelyResponse.Data.ToString() : "Not Found";

                var headers = response.GetHeaderMap();
                headers.Add("Cache-Control", "private");
                headers.Add("Access-Control-Allow-Origin", "*");
                headers.Add("Access-Control-Allow-Methods", "GET,POST");
                headers.Add("Access-Control-Allow-Headers", "Content-Type");
                headers.Add("Content-Type", "application/json; charset=utf-8");
                response.SetHeaderMap(headers);

                response.Status = (int)status;
                response.MimeType = "application/json";
                response.StatusText = (status == HttpStatusCode.OK) ? "OK" : errorStatus;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        /// <summary>
        /// The read response.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="bytesToRead">
        /// The bytes to read.
        /// </param>
        /// <param name="bytesRead">
        /// The bytes read.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            int currBytesRead = 0;

            try
            {
                if (this.mCompleted)
                {
                    bytesRead = 0;
                    this.mTotalBytesRead = 0;
                    this.mResponseBytes = null;
                    return false;
                }
                else
                {
                    if (this.mResponseBytes != null)
                    {
                        currBytesRead = Math.Min(this.mResponseBytes.Length - this.mTotalBytesRead, bytesToRead);
                        response.Write(this.mResponseBytes, this.mTotalBytesRead, currBytesRead);
                        this.mTotalBytesRead += currBytesRead;

                        if (this.mTotalBytesRead >= this.mResponseBytes.Length)
                        {
                            this.mCompleted = true;
                        }
                    }
                    else
                    {
                        bytesRead = 0;
                        this.mCompleted = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            bytesRead = currBytesRead;
            return true;
        }

        /// <summary>
        /// The can get cookie.
        /// </summary>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool CanGetCookie(CefCookie cookie)
        {
            return true;
        }

        /// <summary>
        /// The can set cookie.
        /// </summary>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool CanSetCookie(CefCookie cookie)
        {
            return true;
        }

        /// <summary>
        /// The cancel.
        /// </summary>
        protected override void Cancel()
        {
        }
    }
}