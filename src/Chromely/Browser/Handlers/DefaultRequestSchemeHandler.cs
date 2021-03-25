// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public class DefaultRequestSchemeHandler : CefResourceHandler
    {
        protected readonly IChromelyRouteProvider _routeProvider;
        protected readonly IChromelyRequestSchemeProvider _requestSchemeProvider;
        protected readonly IChromelyRequestTaskRunner _requestTaskRunner;
        protected readonly IChromelySerializerUtil _serializerUtil;
        protected readonly IChromelyErrorHandler _chromelyErrorHandler;

        protected IChromelyResponse _chromelyResponse;
        protected byte[] _responseBytes;
        protected bool _completed;
        protected int _totalBytesRead;

        public DefaultRequestSchemeHandler(IChromelyRouteProvider routeProvider,
                                           IChromelyRequestSchemeProvider requestSchemeProvider,
                                           IChromelyRequestTaskRunner requestTaskRunner,
                                           IChromelySerializerUtil serializerUtil,
                                           IChromelyErrorHandler chromelyErrorHandler)
        {
            _routeProvider = routeProvider;
            _requestSchemeProvider = requestSchemeProvider;
            _requestTaskRunner = requestTaskRunner;
            _serializerUtil = serializerUtil;
            _chromelyErrorHandler = chromelyErrorHandler;
        }

        [Obsolete]
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var isSchemeRegistered = _requestSchemeProvider?.IsSchemeRegistered(request.Url);
            if (isSchemeRegistered.HasValue && isSchemeRegistered.Value)
            {
                var uri = new Uri(request.Url);
                var path = uri.LocalPath;

                bool isRequestAsync = _routeProvider.IsActionRouteAsync(path);
                if (isRequestAsync)
                {
                    ProcessRequestAsync(path);
                }
                else
                {
                    ProcessRequest(path);
                }

                return true;
            }
            else
            {
                _chromelyResponse = _chromelyErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), request.Url);
                string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);
                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
            }

            callback.Dispose();
            return false;

            #region Process Request

            void ProcessRequest(string path)
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                _chromelyResponse = _chromelyErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), path);
                                string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);
                                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                            }
                            else
                            {
                                var parameters = request.Url.GetParameters(request.ReferrerURL);
                                var postData = GetPostData(request);

                                var jsonRequest = _serializerUtil.ObjectToJson(request);
                                _chromelyResponse = _requestTaskRunner.Run(request.Identifier.ToString(), path, parameters, postData, jsonRequest);
                                string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);
                                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                            }
                        }
                        catch (Exception exception)
                        {
                            var chromelyRequest = new ChromelyRequest() { Id = request.Identifier.ToString(), RouteUrl = request.Url };
                            _chromelyResponse = _chromelyErrorHandler.HandleError(chromelyRequest, exception);
                            string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);
                            _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });
            }

            #endregion

            #region Process Request Async

            void ProcessRequestAsync(string path)
            {
                Task.Run(async () =>
                {
                    using (callback)
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                _chromelyResponse = _chromelyErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), path);
                                string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);
                                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                            }
                            else
                            {
                                var parameters = request.Url.GetParameters(request.ReferrerURL);
                                var postData = GetPostData(request);

                                var jsonRequest = _serializerUtil.ObjectToJson(request);
                                _chromelyResponse = await _requestTaskRunner.RunAsync(request.Identifier.ToString(), path, parameters, postData, jsonRequest);
                                string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);

                                _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                            }
                        }
                        catch (Exception exception)
                        {
                            var chromelyRequest = new ChromelyRequest() { Id = request.Identifier.ToString(), RouteUrl = request.Url };
                            _chromelyResponse = _chromelyErrorHandler.HandleError(chromelyRequest, exception);
                            string jsonData = _serializerUtil.EnsureResponseDataIsJson(_chromelyResponse.Data);
                            _responseBytes = Encoding.UTF8.GetBytes(jsonData);
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });
            }

            #endregion
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
                Logger.Instance.Log.LogError(exception, exception.Message);
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
                Logger.Instance.Log.LogError(exception, exception.Message);
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