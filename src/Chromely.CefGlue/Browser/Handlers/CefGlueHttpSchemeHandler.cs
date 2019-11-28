using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    public class CefGlueHttpSchemeHandler : CefResourceHandler
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyRequestTaskRunner _requestTaskRunner;

        private ChromelyResponse _chromelyResponse;
        private byte[] _responseBytes;
        private bool _completed;
        private int _totalBytesRead;

        public CefGlueHttpSchemeHandler(IChromelyConfiguration config, IChromelyRequestTaskRunner requestTaskRunner)
        {
            _config = config;
            _requestTaskRunner = requestTaskRunner;
        }

        [Obsolete]
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var isCustomScheme = _config?.UrlSchemes?.IsUrlRegisteredCustomScheme(request.Url);
            if (isCustomScheme.HasValue && isCustomScheme.Value)
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            var uri = new Uri(request.Url);
                            var path = uri.LocalPath;

                            var response = new ChromelyResponse();
                            if (string.IsNullOrEmpty(path))
                            {
                                response.ReadyState = (int)ReadyState.ResponseIsReady;
                                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                                response.StatusText = "Bad Request";

                                _chromelyResponse = response;
                            }
                            else
                            {
                                var parameters = request.Url.GetParameters();
                                var postData = GetPostData(request);

                                _chromelyResponse = _requestTaskRunner.Run(request.Method, path, parameters, postData);
                                string jsonData = _chromelyResponse.Data.EnsureResponseDataIsJsonFormat();
                                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                            }
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.Log.Error(exception);

                            _chromelyResponse =
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

            Logger.Instance.Log.Error($"Url {request.Url} is not of a registered custom scheme.");
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
                HttpStatusCode status = (_chromelyResponse != null) ? (HttpStatusCode)_chromelyResponse.Status : HttpStatusCode.BadRequest;
                string errorStatus = (_chromelyResponse != null) ? _chromelyResponse.Data.ToString() : "Not Found";

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
                Logger.Instance.Log.Error(exception);
            }
        }

        [Obsolete]
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            int currBytesRead = 0;

            try
            {
                if (_completed)
                {
                    bytesRead = 0;
                    _totalBytesRead = 0;
                    _responseBytes = null;
                    return false;
                }
                else
                {
                    if (_responseBytes != null)
                    {
                        currBytesRead = Math.Min(_responseBytes.Length - _totalBytesRead, bytesToRead);
                        response.Write(_responseBytes, _totalBytesRead, currBytesRead);
                        _totalBytesRead += currBytesRead;

                        if (_totalBytesRead >= _responseBytes.Length)
                        {
                            _completed = true;
                        }
                    }
                    else
                    {
                        bytesRead = 0;
                        _completed = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            bytesRead = currBytesRead;
            return true;
        }

        protected override void Cancel()
        {
        }

        protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
        {
            handleRequest = false;
            return false;
        }

        protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
        {
            bytesSkipped = 0;
            return true;
        }

        protected override bool Read(IntPtr dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
        {
            bytesRead = -1;
            return false;
        }

        private static string GetPostData(CefRequest request)
        {
            var postDataElements = request?.PostData?.GetElements();
            if (postDataElements == null || (postDataElements.Length == 0))
            {
                return string.Empty;
            }

            var dataElement = postDataElements[0];

            switch (dataElement.ElementType)
            {
                case CefPostDataElementType.Empty:
                    break;
                case CefPostDataElementType.File:
                    break;
                case CefPostDataElementType.Bytes:
                    return Encoding.UTF8.GetString(dataElement.GetBytes());
            }

            return string.Empty;
        }
    }
}