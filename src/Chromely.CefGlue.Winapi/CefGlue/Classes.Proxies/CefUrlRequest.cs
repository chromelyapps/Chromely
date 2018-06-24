namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Class used to make a URL request. URL requests are not associated with a
    /// browser instance so no CefClient callbacks will be executed. URL requests
    /// can be created on any valid CEF thread in either the browser or render
    /// process. Once created the methods of the URL request object must be accessed
    /// on the same thread that created it.
    /// </summary>
    public sealed unsafe partial class CefUrlRequest
    {
        /// <summary>
        /// Create a new URL request. Only GET, POST, HEAD, DELETE and PUT request
        /// methods are supported. Multiple post data elements are not supported and
        /// elements of type PDE_TYPE_FILE are only supported for requests originating
        /// from the browser process. Requests originating from the render process will
        /// receive the same handling as requests originating from Web content -- if
        /// the response contains Content-Disposition or Mime-Type header values that
        /// would not normally be rendered then the response may receive special
        /// handling inside the browser (for example, via the file download code path
        /// instead of the URL request code path). The |request| object will be marked
        /// as read-only after calling this method. In the browser process if
        /// |request_context| is empty the global request context will be used. In the
        /// render process |request_context| must be empty and the context associated
        /// with the current renderer process' browser will be used.
        /// </summary>
        public static CefUrlRequest Create(CefRequest request, CefUrlRequestClient client, CefRequestContext requestContext)
        {
            if (request == null) throw new ArgumentNullException("request");

            var n_request = request.ToNative();
            var n_client = client.ToNative();
            var n_requestContext = requestContext != null ? requestContext.ToNative() : null;

            return CefUrlRequest.FromNative(
                cef_urlrequest_t.create(n_request, n_client, n_requestContext)
                );
        }

        /// <summary>
        /// Returns the request object used to create this URL request. The returned
        /// object is read-only and should not be modified.
        /// </summary>
        public CefRequest GetRequest()
        {
            return CefRequest.FromNative(
                cef_urlrequest_t.get_request(_self)
                );
        }

        /// <summary>
        /// Returns the client.
        /// </summary>
        public CefUrlRequestClient GetClient()
        {
            return CefUrlRequestClient.FromNative(
                cef_urlrequest_t.get_client(_self)
                );
        }

        /// <summary>
        /// Returns the request status.
        /// </summary>
        public CefUrlRequestStatus RequestStatus
        {
            get { return cef_urlrequest_t.get_request_status(_self); }
        }

        /// <summary>
        /// Returns the request error if status is UR_CANCELED or UR_FAILED, or 0
        /// otherwise.
        /// </summary>
        public CefErrorCode RequestError
        {
            get { return cef_urlrequest_t.get_request_error(_self); }
        }

        /// <summary>
        /// Returns the response, or NULL if no response information is available.
        /// Response information will only be available after the upload has completed.
        /// The returned object is read-only and should not be modified.
        /// </summary>
        public CefResponse GetResponse()
        {
            return CefResponse.FromNativeOrNull(
                cef_urlrequest_t.get_response(_self)
                );
        }

        /// <summary>
        /// Returns true if the response body was served from the cache. This includes
        /// responses for which revalidation was required.
        /// </summary>
        public bool ResponseWasCached
        {
            get
            {
                return cef_urlrequest_t.response_was_cached(_self) != 0;
            }
        }

        /// <summary>
        /// Cancel the request.
        /// </summary>
        public void Cancel()
        {
            cef_urlrequest_t.cancel(_self);
        }
    }
}
