// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpRequestHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography.X509Certificates;
using global::CefSharp;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    /// <summary>
    /// The CefSharp request handler.
    /// </summary>
    public class CefSharpRequestHandler : IRequestHandler
    {
        /// <summary>
        /// The on before browse.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="userGesture">
        /// The user Gesture.
        /// </param>
        /// <param name="isRedirect">
        /// The is redirect.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            bool isUrlExternal = UrlSchemeProvider.IsUrlRegisteredExternal(request.Url);
            if (isUrlExternal)
            {
                System.Diagnostics.Process.Start(request.Url);
                return true;
            }

            var isUrlCommand = UrlSchemeProvider.IsUrlRegisteredCommand(request.Url);
            if (isUrlCommand)
            {
                CommandTaskRunner.RunAsync(request.Url);
                return true;
            }

            return false;
        }

        /// <summary>
        /// The on open url from tab.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="targetUrl">
        /// The target url.
        /// </param>
        /// <param name="targetDisposition">
        /// The target disposition.
        /// </param>
        /// <param name="userGesture">
        /// The user gesture.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        /// <summary>
        /// The on certificate error.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <param name="requestUrl">
        /// The request url.
        /// </param>
        /// <param name="sslInfo">
        /// The ssl info.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    // To allow certificate
                    // callback.Continue(true);
                    // return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The on plugin crashed.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="pluginPath">
        /// The plugin path.
        /// </param>
        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
        }

        /// <summary>
        /// The on before resource load.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="CefReturnValue"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Generic exception - Request not a valid url.
        /// </exception>
        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out _) == false)
            {
                throw new Exception("Request to \"" + request.Url + "\" can't continue, not a valid URI");
            }

            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    if (request.Method == "POST")
                    {
                        using (var postData = request.PostData)
                        {
                            if (postData != null)
                            {
                                var elements = postData.Elements;

                                var charSet = request.GetCharSet();

                                foreach (var element in elements)
                                {
                                    if (element.Type == PostDataElementType.Bytes)
                                    {
                                        element.GetBody(charSet);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return CefReturnValue.Continue;
        }

        /// <summary>
        /// The get auth credentials.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="isProxy">
        /// The is proxy.
        /// </param>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="realm">
        /// The realm.
        /// </param>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// The on select client certificate.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="isProxy">
        /// The is proxy.
        /// </param>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="certificates">
        /// The certificates.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// The on render process terminated.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
        }

        /// <summary>
        /// The can get cookies.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanGetCookies(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            return true;
        }

        /// <summary>
        /// The can set cookie.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSetCookie(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
        {
            return true;
        }

        /// <summary>
        /// The on quota request.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="originUrl">
        /// The origin url.
        /// </param>
        /// <param name="newSize">
        /// The new size.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    // Accept Request to raise Quota
                    // callback.Continue(true);
                    // return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The on resource redirect.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="newUrl">
        /// The new url.
        /// </param>
        void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
        }

        /// <summary>
        /// The on protocol execution.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return url.StartsWith("mailto");
        }

        /// <summary>
        /// The on render view ready.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
        }

        /// <summary>
        /// The on resource response.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IRequestHandler.OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            // NOTE: You cannot modify the response, only the request
            // You can now access the headers
            return false;
        }

        /// <summary>
        /// The get resource response filter.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="IResponseFilter"/>.
        /// </returns>
        IResponseFilter IRequestHandler.GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }

        /// <summary>
        /// The on resource load complete.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <param name="receivedContentLength">
        /// The received content length.
        /// </param>
        void IRequestHandler.OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
        }
    }
}