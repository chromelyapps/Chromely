namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;
    using System.IO;

    /// <summary>
    /// Class used to implement a custom request handler interface. The methods of
    /// this class will always be called on the IO thread.
    /// </summary>
    public abstract unsafe partial class CefResourceHandler
    {
        private volatile bool _keepObject;

        public void KeepObject()
        {
            if (!_keepObject)
            {
                lock (SyncRoot)
                {
                    if (!_keepObject)
                    {
                        add_ref(_self);
                        _keepObject = true;
                    }
                }
            }
        }

        public void ReleaseObject()
        {
            if (_keepObject)
            {
                lock (SyncRoot)
                {
                    if (_keepObject)
                    {
                        release(_self);
                        _keepObject = false;
                    }
                }
            }
        }


        private int process_request(cef_resource_handler_t* self, cef_request_t* request, cef_callback_t* callback)
        {
            CheckSelf(self);

            var m_request = CefRequest.FromNative(request);
            var m_callback = CefCallback.FromNative(callback);

            var result = ProcessRequest(m_request, m_callback);
            return result ? 1 : 0;
        }

        /// <summary>
        /// Begin processing the request. To handle the request return true and call
        /// CefCallback::Continue() once the response header information is available
        /// (CefCallback::Continue() can also be called from inside this method if
        /// header information is available immediately). To cancel the request return
        /// false.
        /// </summary>
        protected abstract bool ProcessRequest(CefRequest request, CefCallback callback);


        private void get_response_headers(cef_resource_handler_t* self, cef_response_t* response, long* response_length, cef_string_t* redirectUrl)
        {
            CheckSelf(self);

            var m_response = CefResponse.FromNative(response);
            long m_responseLength;
            string m_redirectUrl;

            GetResponseHeaders(m_response, out m_responseLength, out m_redirectUrl);

            *response_length = m_responseLength;

            if (!string.IsNullOrEmpty(m_redirectUrl))
            {
                cef_string_t.Copy(m_redirectUrl, redirectUrl);
            }
        }

        /// <summary>
        /// Retrieve response header information. If the response length is not known
        /// set |response_length| to -1 and ReadResponse() will be called until it
        /// returns false. If the response length is known set |response_length|
        /// to a positive value and ReadResponse() will be called until it returns
        /// false or the specified number of bytes have been read. Use the |response|
        /// object to set the mime type, http status code and other optional header
        /// values. To redirect the request to a new URL set |redirectUrl| to the new
        /// URL. If an error occured while setting up the request you can call
        /// SetError() on |response| to indicate the error condition.
        /// </summary>
        protected abstract void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl);


        private int read_response(cef_resource_handler_t* self, void* data_out, int bytes_to_read, int* bytes_read, cef_callback_t* callback)
        {
            CheckSelf(self);

            var m_callback = CefCallback.FromNative(callback);

            using (var m_stream = new UnmanagedMemoryStream((byte*)data_out, bytes_to_read, bytes_to_read, FileAccess.Write))
            {
                int m_bytesRead;
                var result = ReadResponse(m_stream, bytes_to_read, out m_bytesRead, m_callback);
                *bytes_read = m_bytesRead;
                return result ? 1 : 0;
            }
        }

        /// <summary>
        /// Read response data. If data is available immediately copy up to
        /// |bytes_to_read| bytes into |data_out|, set |bytes_read| to the number of
        /// bytes copied, and return true. To read the data at a later time set
        /// |bytes_read| to 0, return true and call CefCallback::Continue() when the
        /// data is available. To indicate response completion return false.
        /// </summary>
        protected abstract bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback);


        private int can_get_cookie(cef_resource_handler_t* self, cef_cookie_t* cookie)
        {
            CheckSelf(self);

            var m_cookie = CefCookie.FromNative(cookie);

            return CanGetCookie(m_cookie) ? 1 : 0;
        }

        /// <summary>
        /// Return true if the specified cookie can be sent with the request or false
        /// otherwise. If false is returned for any cookie then no cookies will be sent
        /// with the request.
        /// </summary>
        protected abstract bool CanGetCookie(CefCookie cookie);


        private int can_set_cookie(cef_resource_handler_t* self, cef_cookie_t* cookie)
        {
            CheckSelf(self);

            var m_cookie = CefCookie.FromNative(cookie);

            return CanSetCookie(m_cookie) ? 1 : 0;
        }

        /// <summary>
        /// Return true if the specified cookie returned with the response can be set
        /// or false otherwise.
        /// </summary>
        protected abstract bool CanSetCookie(CefCookie cookie);


        private void cancel(cef_resource_handler_t* self)
        {
            CheckSelf(self);

            Cancel();
        }

        /// <summary>
        /// Request processing has been canceled.
        /// </summary>
        protected abstract void Cancel();
    }
}
