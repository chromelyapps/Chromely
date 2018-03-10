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

namespace Chromely.CefGlue.Winapi.Browser.Handlers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    internal sealed class CefGlueResourceSchemeHandler : CefResourceHandler
    {
        private Byte[] m_fileBytes;
        private string m_mime;
        private bool m_completed;
        private int m_totalBytesRead;


        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            Uri u = new Uri(request.Url);
            String file = u.Authority + u.AbsolutePath;

            m_totalBytesRead = 0;
            m_fileBytes = null;
            m_completed = false;

            if (File.Exists(file))
            {
                Task.Factory.StartNew(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            m_fileBytes = File.ReadAllBytes(file);

                            string extension = Path.GetExtension(file);
                            m_mime = MimeMapper.GetMimeType(extension);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });

                return true;
            }

            callback.Dispose();
            return false;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            // unknown content-length
            // no-redirect
            responseLength = -1;
            redirectUrl = null;

            try
            {
                var headers = response.GetHeaderMap();
                headers.Add("Access-Control-Allow-Origin", "*");
                response.SetHeaderMap(headers);

                response.Status = (int)HttpStatusCode.OK;
                response.MimeType = m_mime;
                response.StatusText = "OK";
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.BadRequest; 
                response.MimeType = "text/plain";
                response.StatusText = "Resource loading error.";

                Log.Error(exception);
            }
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            int currBytesRead = 0;

            try
            {
                if (m_completed)
                {
                    bytesRead = 0;
                    m_totalBytesRead = 0;
                    m_fileBytes = null;
                    return false;
                }
                else
                {
                    if (m_fileBytes != null)
                    {
                        currBytesRead = Math.Min(m_fileBytes.Length - m_totalBytesRead, bytesToRead);
                        response.Write(m_fileBytes, m_totalBytesRead, currBytesRead);
                        m_totalBytesRead += currBytesRead;

                        if (m_totalBytesRead >= m_fileBytes.Length)
                        {
                            m_completed = true;
                        }
                    }
                    else
                    {
                        bytesRead = 0;
                        m_completed = true;
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
