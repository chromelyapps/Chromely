/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.CefGlueBrowser
{
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using LitJson;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using Xilium.CefGlue;

    public sealed class CefGlueDefaultSchemeHandler : CefResourceHandler
    {
        private bool m_completed;
        private int m_bytesRead;
        private TaskData m_chromelyResponse;

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            try
            {
                m_chromelyResponse = new TaskData();
                CefGlueRequestTaskRunner.Run(request, callback, m_chromelyResponse);
            }
            catch (Exception exception)
            {
                Log.Error(exception);

                m_chromelyResponse = new TaskData();
                m_chromelyResponse.StatusCode = HttpStatusCode.BadRequest;
                m_chromelyResponse.Data = "An error occured.";
            }

            return true;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            // unknown content-length
            // no-redirect
            responseLength = -1;
            redirectUrl = null;

            try
            {
                HttpStatusCode status = (m_chromelyResponse != null) ? m_chromelyResponse.StatusCode : HttpStatusCode.BadRequest;
                string errorStatus = (m_chromelyResponse != null) ? m_chromelyResponse.Data.ToString() : "Not Found";

                var headers = response.GetHeaderMap();
                headers.Add("Cache-Control", "private");
                headers.Add("Access-Control-Allow-Origin", "*");
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

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            try
            {
                if (m_completed)
                {
                    bytesRead = m_bytesRead;
                    return false;
                }
                else
                {
                    m_bytesRead = 0;
                    bytesRead = m_bytesRead;

                    string jsonData = JsonMapper.ToJson(m_chromelyResponse.Data);

                    var content = Encoding.UTF8.GetBytes(jsonData);
                    if (bytesToRead < content.Length) throw new NotImplementedException();
                    response.Write(content, 0, content.Length);
                    m_bytesRead = content.Length;
                    bytesRead = m_bytesRead;

                    m_completed = true;
                }

            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            bytesRead = m_bytesRead;
            return true;
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return true;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return true;
        }

        protected override void Cancel()
        {
        }
    }
}

