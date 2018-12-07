namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Implement this interface to handle events related to browser requests. The
    /// methods of this class will be called on the thread indicated.
    /// </summary>
    public abstract unsafe partial class CefRequestHandler
    {
        private int on_before_browse(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, int user_gesture, int is_redirect)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_request = CefRequest.FromNative(request);
            var m_userGesture = user_gesture != 0;
            var m_isRedirect = is_redirect != 0;

            var result = OnBeforeBrowse(m_browser, m_frame, m_request, m_userGesture, m_isRedirect);

            return result ? 1 : 0;
        }

        /// <summary>
        /// Called on the UI thread before browser navigation. Return true to cancel
        /// the navigation or false to allow the navigation to proceed. The |request|
        /// object cannot be modified in this callback.
        /// CefLoadHandler::OnLoadingStateChange will be called twice in all cases.
        /// If the navigation is allowed CefLoadHandler::OnLoadStart and
        /// CefLoadHandler::OnLoadEnd will be called. If the navigation is canceled
        /// CefLoadHandler::OnLoadError will be called with an |errorCode| value of
        /// ERR_ABORTED. The |user_gesture| value will be true if the browser
        /// navigated via explicit user gesture (e.g. clicking a link) or false if it
        /// navigated automatically (e.g. via the DomContentLoaded event).
        /// </summary>
        protected virtual bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }


        private int on_open_urlfrom_tab(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_string_t* target_url, CefWindowOpenDisposition target_disposition, int user_gesture)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_targetUrl = cef_string_t.ToString(target_url);
            var m_userGesture = user_gesture != 0;

            var m_result = OnOpenUrlFromTab(m_browser, m_frame, m_targetUrl, target_disposition, m_userGesture);

            return m_result ? 1 : 0;
        }

        /// <summary>
        /// Called on the UI thread before OnBeforeBrowse in certain limited cases
        /// where navigating a new or different browser might be desirable. This
        /// includes user-initiated navigation that might open in a special way (e.g.
        /// links clicked via middle-click or ctrl + left-click) and certain types of
        /// cross-origin navigation initiated from the renderer process (e.g.
        /// navigating the top-level frame to/from a file URL). The |browser| and
        /// |frame| values represent the source of the navigation. The
        /// |target_disposition| value indicates where the user intended to navigate
        /// the browser based on standard Chromium behaviors (e.g. current tab,
        /// new tab, etc). The |user_gesture| value will be true if the browser
        /// navigated via explicit user gesture (e.g. clicking a link) or false if it
        /// navigated automatically (e.g. via the DomContentLoaded event). Return true
        /// to cancel the navigation or false to allow the navigation to proceed in the
        /// source browser's top-level frame.
        /// </summary>
        protected virtual bool OnOpenUrlFromTab(CefBrowser browser, CefFrame frame, string targetUrl, CefWindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }


        private CefReturnValue on_before_resource_load(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, cef_request_callback_t* callback)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_request = CefRequest.FromNative(request);
            var m_callback = CefRequestCallback.FromNative(callback);

            var result = OnBeforeResourceLoad(m_browser, m_frame, m_request, m_callback);

            if (result != CefReturnValue.ContinueAsync)
            {
                m_browser.Dispose();
                m_frame.Dispose();
                m_request.Dispose();
                m_callback.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Called on the IO thread before a resource request is loaded. The |request|
        /// object may be modified. Return RV_CONTINUE to continue the request
        /// immediately. Return RV_CONTINUE_ASYNC and call CefRequestCallback::
        /// Continue() at a later time to continue or cancel the request
        /// asynchronously. Return RV_CANCEL to cancel the request immediately.
        /// </summary>
        protected virtual CefReturnValue OnBeforeResourceLoad(CefBrowser browser, CefFrame frame, CefRequest request, CefRequestCallback callback)
        {
            return CefReturnValue.Continue;
        }


        private cef_resource_handler_t* get_resource_handler(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_request = CefRequest.FromNative(request);

            var handler = GetResourceHandler(m_browser, m_frame, m_request);

            m_request.Dispose();

            return handler != null ? handler.ToNative() : null;
        }

        /// <summary>
        /// Called on the IO thread before a resource is loaded. To allow the resource
        /// to load normally return NULL. To specify a handler for the resource return
        /// a CefResourceHandler object. The |request| object should not be modified in
        /// this callback.
        /// </summary>
        protected virtual CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            return null;
        }


        private void on_resource_redirect(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, cef_response_t* response, cef_string_t* new_url)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_request = CefRequest.FromNative(request);
            var m_response = CefResponse.FromNative(response);
            var m_newUrl = cef_string_t.ToString(new_url);

            var o_newUrl = m_newUrl;
            OnResourceRedirect(m_browser, m_frame, m_request, m_response, ref m_newUrl);

            if ((object)m_newUrl != (object)o_newUrl)
            {
                cef_string_t.Copy(m_newUrl, new_url);
            }
        }

        /// <summary>
        /// Called on the IO thread when a resource load is redirected. The |request|
        /// parameter will contain the old URL and other request-related information.
        /// The |response| parameter will contain the response that resulted in the
        /// redirect. The |new_url| parameter will contain the new URL and can be
        /// changed if desired. The |request| object cannot be modified in this
        /// callback.
        /// </summary>
        protected virtual void OnResourceRedirect(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response, ref string newUrl)
        {
        }


        private int on_resource_response(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, cef_response_t* response)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_request = CefRequest.FromNative(request);
            var m_response = CefResponse.FromNative(response);

            var m_result = OnResourceResponse(m_browser, m_frame, m_request, m_response);

            return m_result ? 1 : 0;
        }

        /// <summary>
        /// Called on the IO thread when a resource response is received. To allow the
        /// resource to load normally return false. To redirect or retry the resource
        /// modify |request| (url, headers or post body) and return true. The
        /// |response| object cannot be modified in this callback.
        /// </summary>
        protected virtual bool OnResourceResponse(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
        {
            return false;
        }


        private cef_response_filter_t* get_resource_response_filter(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, cef_response_t* response)
        {
            CheckSelf(self);

            var mBrowser = CefBrowser.FromNative(browser);
            var mFrame = CefFrame.FromNative(frame);
            var mRequest = CefRequest.FromNative(request);
            var mResponse = CefResponse.FromNative(response);

            var result = GetResourceResponseFilter(mBrowser, mFrame, mRequest, mResponse);

            if (result != null)
            {
                return result.ToNative();
            }

            return null;
        }

        /// <summary>
        /// Called on the IO thread to optionally filter resource response content.
        /// |request| and |response| represent the request and response respectively
        /// and cannot be modified in this callback.
        /// </summary>
        protected virtual CefResponseFilter GetResourceResponseFilter(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response)
        {
            return null;
        }


        private void on_resource_load_complete(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, cef_response_t* response, CefUrlRequestStatus status, long received_content_length)
        {
            CheckSelf(self);

            var mBrowser = CefBrowser.FromNative(browser);
            var mFrame = CefFrame.FromNative(frame);
            var mRequest = CefRequest.FromNative(request);
            var mResponse = CefResponse.FromNative(response);

            OnResourceLoadComplete(mBrowser, mFrame, mRequest, mResponse, status, received_content_length);
        }

        /// <summary>
        /// Called on the IO thread when a resource load has completed. |request| and
        /// |response| represent the request and response respectively and cannot be
        /// modified in this callback. |status| indicates the load completion status.
        /// |received_content_length| is the number of response bytes actually read.
        /// </summary>
        protected virtual void OnResourceLoadComplete(CefBrowser browser, CefFrame frame, CefRequest request, CefResponse response, CefUrlRequestStatus status, long receivedContentLength)
        {
        }


        private int get_auth_credentials(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, int isProxy, cef_string_t* host, int port, cef_string_t* realm, cef_string_t* scheme, cef_auth_callback_t* callback)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_frame = CefFrame.FromNative(frame);
            var m_host = cef_string_t.ToString(host);
            var m_realm = cef_string_t.ToString(realm);
            var m_scheme = cef_string_t.ToString(scheme);
            var m_callback = CefAuthCallback.FromNative(callback);

            var result = GetAuthCredentials(m_browser, m_frame, isProxy != 0, m_host, port, m_realm, m_scheme, m_callback);

            return result ? 1 : 0;
        }

        /// <summary>
        /// Called on the IO thread when the browser needs credentials from the user.
        /// |isProxy| indicates whether the host is a proxy server. |host| contains the
        /// hostname and |port| contains the port number. |realm| is the realm of the
        /// challenge and may be empty. |scheme| is the authentication scheme used,
        /// such as "basic" or "digest", and will be empty if the source of the request
        /// is an FTP server. Return true to continue the request and call
        /// CefAuthCallback::Continue() either in this method or at a later time when
        /// the authentication information is available. Return false to cancel the
        /// request immediately.
        /// </summary>
        protected virtual bool GetAuthCredentials(CefBrowser browser, CefFrame frame, bool isProxy, string host, int port, string realm, string scheme, CefAuthCallback callback)
        {
            return false;
        }


        private int can_get_cookies(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request)
        {
            CheckSelf(self);

            var mBrowser = CefBrowser.FromNative(browser);
            var mFrame = CefFrame.FromNative(frame);
            var mRequest = CefRequest.FromNative(request);

            var mResult = CanGetCookies(mBrowser, mFrame, mRequest);

            return mResult ? 1 : 0;
        }

        /// <summary>
        /// Called on the IO thread before sending a network request with a "Cookie"
        /// request header. Return true to allow cookies to be included in the network
        /// request or false to block cookies. The |request| object should not be
        /// modified in this callback.
        /// </summary>
        protected virtual bool CanGetCookies(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            return true;
        }


        private int can_set_cookie(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, cef_cookie_t* cookie)
        {
            CheckSelf(self);

            var mBrowser = CefBrowser.FromNative(browser);
            var mFrame = CefFrame.FromNative(frame);
            var mRequest = CefRequest.FromNative(request);
            var mCookie = CefCookie.FromNative(cookie);

            var mResult = CanSetCookie(mBrowser, mFrame, mRequest, mCookie);

            return mResult ? 1 : 0;
        }

        /// <summary>
        /// Called on the IO thread when receiving a network request with a
        /// "Set-Cookie" response header value represented by |cookie|. Return true to
        /// allow the cookie to be stored or false to block the cookie. The |request|
        /// object should not be modified in this callback.
        /// </summary>
        protected virtual bool CanSetCookie(CefBrowser browser, CefFrame frame, CefRequest request, CefCookie cookie)
        {
            return true;
        }


        private int on_quota_request(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* origin_url, long new_size, cef_request_callback_t* callback)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_origin_url = cef_string_t.ToString(origin_url);
            var m_callback = CefRequestCallback.FromNative(callback);

            var result = OnQuotaRequest(m_browser, m_origin_url, new_size, m_callback);

            return result ? 1 : 0;
        }

        /// <summary>
        /// Called on the IO thread when JavaScript requests a specific storage quota
        /// size via the webkitStorageInfo.requestQuota function. |origin_url| is the
        /// origin of the page making the request. |new_size| is the requested quota
        /// size in bytes. Return true to continue the request and call
        /// CefRequestCallback::Continue() either in this method or at a later time to
        /// grant or deny the request. Return false to cancel the request immediately.
        /// </summary>
        protected virtual bool OnQuotaRequest(CefBrowser browser, string originUrl, long newSize, CefRequestCallback callback)
        {
            callback.Continue(true);
            return true;
        }


        private void on_protocol_execution(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* url, int* allow_os_execution)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_url = cef_string_t.ToString(url);
            bool m_allow_os_execution;

            OnProtocolExecution(m_browser, m_url, out m_allow_os_execution);

            *allow_os_execution = m_allow_os_execution ? 1 : 0;
        }

        /// <summary>
        /// Called on the UI thread to handle requests for URLs with an unknown
        /// protocol component. Set |allow_os_execution| to true to attempt execution
        /// via the registered OS protocol handler, if any.
        /// SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS BASED
        /// ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        protected virtual void OnProtocolExecution(CefBrowser browser, string url, out bool allowOSExecution)
        {
            allowOSExecution = true;
        }


        private int on_certificate_error(cef_request_handler_t* self, cef_browser_t* browser, CefErrorCode cert_error, cef_string_t* request_url, cef_sslinfo_t* ssl_info, cef_request_callback_t* callback)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_request_url = cef_string_t.ToString(request_url);
            var m_ssl_info = CefSslInfo.FromNative(ssl_info);
            var m_callback = CefRequestCallback.FromNativeOrNull(callback);

            var result = OnCertificateError(m_browser, cert_error, m_request_url, m_ssl_info, m_callback);

            return result ? 1 : 0;
        }

        /// <summary>
        /// Called on the UI thread to handle requests for URLs with an invalid
        /// SSL certificate. Return true and call CefRequestCallback::Continue() either
        /// in this method or at a later time to continue or cancel the request. Return
        /// false to cancel the request immediately. If
        /// CefSettings.ignore_certificate_errors is set all invalid certificates will
        /// be accepted without calling this method.
        /// </summary>
        protected virtual bool OnCertificateError(CefBrowser browser, CefErrorCode certError, string requestUrl, CefSslInfo sslInfo, CefRequestCallback callback)
        {
            return false;
        }


        private int on_select_client_certificate(cef_request_handler_t* self, cef_browser_t* browser, int isProxy, cef_string_t* host, int port, UIntPtr certificatesCount, cef_x509certificate_t** certificates, cef_select_client_certificate_callback_t* callback)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_isProxy = isProxy != 0;
            var m_host = cef_string_t.ToString(host);
            var m_certCount = checked((int)certificatesCount);
            var m_certificates = new CefX509Certificate[m_certCount];
            for (var i = 0; i < m_certCount; i++)
            {
                m_certificates[i] = CefX509Certificate.FromNative(certificates[i]);
            }
            var m_callback = CefSelectClientCertificateCallback.FromNative(callback);

            var result = OnSelectClientCertificate(m_browser, m_isProxy, m_host, port, m_certificates, m_callback);
            if (result)
            {
                return 1;
            }
            else
            {
                m_callback.Dispose();
                return 0;
            }
        }

        /// <summary>
        /// Called on the UI thread when a client certificate is being requested for
        /// authentication. Return false to use the default behavior and automatically
        /// select the first certificate available. Return true and call
        /// CefSelectClientCertificateCallback::Select either in this method or at a
        /// later time to select a certificate. Do not call Select or call it with NULL
        /// to continue without using any certificate. |isProxy| indicates whether the
        /// host is an HTTPS proxy or the origin server. |host| and |port| contains the
        /// hostname and port of the SSL server. |certificates| is the list of
        /// certificates to choose from; this list has already been pruned by Chromium
        /// so that it only contains certificates from issuers that the server trusts.
        /// </summary>
        protected virtual bool OnSelectClientCertificate(CefBrowser browser, bool isProxy, string host, int port, CefX509Certificate[] certificates, CefSelectClientCertificateCallback callback)
        {
            return false;
        }


        private void on_plugin_crashed(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* plugin_path)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            var m_plugin_path = cef_string_t.ToString(plugin_path);

            OnPluginCrashed(m_browser, m_plugin_path);
        }

        /// <summary>
        /// Called on the browser process UI thread when a plugin has crashed.
        /// |plugin_path| is the path of the plugin that crashed.
        /// </summary>
        protected virtual void OnPluginCrashed(CefBrowser browser, string pluginPath)
        {
        }


        private void on_render_view_ready(cef_request_handler_t* self, cef_browser_t* browser)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);
            OnRenderViewReady(m_browser);
        }

        /// <summary>
        /// Called on the browser process UI thread when the render view associated
        /// with |browser| is ready to receive/handle IPC messages in the render
        /// process.
        /// </summary>
        protected virtual void OnRenderViewReady(CefBrowser browser)
        {
        }


        private void on_render_process_terminated(cef_request_handler_t* self, cef_browser_t* browser, CefTerminationStatus status)
        {
            CheckSelf(self);

            var m_browser = CefBrowser.FromNative(browser);

            OnRenderProcessTerminated(m_browser, status);
        }

        /// <summary>
        /// Called on the browser process UI thread when the render process
        /// terminates unexpectedly. |status| indicates how the process
        /// terminated.
        /// </summary>
        protected virtual void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
        {
        }
    }
}
