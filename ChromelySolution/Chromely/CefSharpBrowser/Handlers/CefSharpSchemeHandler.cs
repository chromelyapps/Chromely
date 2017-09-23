namespace Chromely.CefSharpBrowser.Handlers
{
    using CefSharp;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using System.Net;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using LitJson;
    using Chromely.RestfulService;

    public class CefSharpSchemeHandler : ResourceHandler
    {
        private ChromelyResponse m_chromelyResponse;
        private string m_mimeType;
        private Stream m_stream = new MemoryStream();

        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            bool isCustomScheme = UrlSchemeProvider.IsUrlOfRegisteredCustomScheme(request.Url);
            if (isCustomScheme)
            {
                Task.Factory.StartNew(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            m_chromelyResponse = CefSharpRequestTaskRunner.Run(request);

                            string jsonData = m_chromelyResponse.Data.EnsureResponseIsJsonFormat();
                            var content = Encoding.UTF8.GetBytes(jsonData);
                            m_stream.Write(content, 0, content.Length);
                            m_mimeType = "application/json";
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

            Log.Error(string.Format("Url {0} is not of a registered custom scheme.", request.Url));
            callback.Dispose();
            return false;
        }

        public override Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = m_stream.Length;
            redirectUrl = null;

            var headers = response.ResponseHeaders;
            headers.Add("Cache-Control", "private");
            headers.Add("Access-Control-Allow-Origin", "*");
            headers.Add("Content-Type", "application/json; charset=utf-8");

            response.StatusCode = m_chromelyResponse.Status;
            response.StatusText = m_chromelyResponse.StatusText;
            response.MimeType = m_mimeType; 

            return m_stream;
        }
    }
}
