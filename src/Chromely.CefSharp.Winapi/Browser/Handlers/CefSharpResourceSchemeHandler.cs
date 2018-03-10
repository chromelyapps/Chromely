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

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using System.Net;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;

    using global::CefSharp;

    public class CefSharpResourceSchemeHandler : ResourceHandler
    {
        private ChromelyResponse m_chromelyResponse;
        private string m_mimeType;
        private Stream m_stream = new MemoryStream();

        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            Uri u = new Uri(request.Url);
            String file = u.Authority + u.AbsolutePath;

            if (File.Exists(file))
            {
                Task.Factory.StartNew(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            m_chromelyResponse = new ChromelyResponse();
                            m_chromelyResponse.Status = (int)HttpStatusCode.OK;
                            m_chromelyResponse.Data = "OK.";

                            Byte[] fileBytes = File.ReadAllBytes(file);
                            m_stream = new MemoryStream(fileBytes);

                            string extension = Path.GetExtension(file);
                            m_mimeType = MimeMapper.GetMimeType(extension);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);

                            m_chromelyResponse = new ChromelyResponse();
                            m_chromelyResponse.Status = (int)HttpStatusCode.BadRequest;
                            m_chromelyResponse.Data = "An error occured.";
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });

                return true;
            }

            Log.Error(string.Format("File {0} is not valid.", request.Url));
            callback.Dispose();
            return false;
        }

        public override Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = m_stream.Length;
            redirectUrl = null;

            response.StatusCode = m_chromelyResponse.Status;
            response.StatusText = m_chromelyResponse.StatusText;
            response.MimeType = m_mimeType;

            return m_stream;
        }
    }
}
