// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueHttpSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Chromely.CefGlue.Gtk.RestfulService;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue http scheme handler.
    /// </summary>
    public class CefGlueHttpSchemeHandler : CefResourceHandler
    {
        /// <summary>
        /// The ChromelyResponse response.
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
                            mChromelyResponse = RequestTaskRunner.Run(request);
                            string jsonData = mChromelyResponse.Data.EnsureResponseIsJsonFormat();
                            mResponseBytes = Encoding.UTF8.GetBytes(jsonData);
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
                HttpStatusCode status = (mChromelyResponse != null) ? (HttpStatusCode)mChromelyResponse.Status : HttpStatusCode.BadRequest;
                string errorStatus = (mChromelyResponse != null) ? mChromelyResponse.Data.ToString() : "Not Found";

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
                if (mCompleted)
                {
                    bytesRead = 0;
                    mTotalBytesRead = 0;
                    mResponseBytes = null;
                    return false;
                }
                else
                {
                    if (mResponseBytes != null)
                    {
                        currBytesRead = Math.Min(mResponseBytes.Length - mTotalBytesRead, bytesToRead);
                        response.Write(mResponseBytes, mTotalBytesRead, currBytesRead);
                        mTotalBytesRead += currBytesRead;

                        if (mTotalBytesRead >= mResponseBytes.Length)
                        {
                            mCompleted = true;
                        }
                    }
                    else
                    {
                        bytesRead = 0;
                        mCompleted = true;
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
