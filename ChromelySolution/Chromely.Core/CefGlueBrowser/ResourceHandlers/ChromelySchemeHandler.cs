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
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using Xilium.CefGlue;

    internal sealed class ChromelySchemeHandler : CefResourceHandler
    {
        private bool m_completed;
        private TaskData m_chromelyResponse;
        private static ILogger Logger = LoggerFactory.GetLogger();

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            try
            {
                m_chromelyResponse = new TaskData();
                RequestTaskRunner.Run(request, callback, m_chromelyResponse);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception);

                m_chromelyResponse = new TaskData();
                m_chromelyResponse.StatusCode = HttpStatusCode.BadRequest;
                m_chromelyResponse.JsonData = "An error occured.";
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
                string errorStatus = (m_chromelyResponse != null) ? m_chromelyResponse.JsonData : "Not Found";

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
                Console.Write(exception.Message);
            }
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            bytesRead = 0;

            try
            {
                if (m_completed)
                {
                    return false;
                }
                else
                {
                    string jsonData = m_chromelyResponse.JsonData;

                    var content = Encoding.UTF8.GetBytes(jsonData);
                    if (bytesToRead < content.Length) throw new NotImplementedException();
                    response.Write(content, 0, content.Length);
                    bytesRead = content.Length;

                    m_completed = true;
                }

            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }

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

