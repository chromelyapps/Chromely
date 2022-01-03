// Copyright © 2017 Chromely Projects. All rights reserved.
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
    public class DefaultRequestSchemeHandler : ResourceHandler
    {
        protected readonly IChromelyRouteProvider _routeProvider;
        protected readonly IChromelyRequestSchemeProvider _requestSchemeProvider;
        protected readonly IChromelyRequestHandler _requestHandler;
        protected readonly IChromelyDataTransferOptions _dataTransferOptions;
        protected readonly IChromelyErrorHandler _chromelyErrorHandler;

        protected IChromelyResponse _chromelyResponse;
        protected Stream _stream;
        protected string _mimeType;

        public DefaultRequestSchemeHandler(IChromelyRouteProvider routeProvider,
                                           IChromelyRequestSchemeProvider requestSchemeProvider,
                                           IChromelyRequestHandler requestHandler,
                                           IChromelyDataTransferOptions dataTransferOptions,
                                           IChromelyErrorHandler chromelyErrorHandler)
        {
            _routeProvider = routeProvider;
            _requestSchemeProvider = requestSchemeProvider;
            _requestHandler = requestHandler;
            _dataTransferOptions = dataTransferOptions;
            _chromelyErrorHandler = chromelyErrorHandler;
            _chromelyResponse = new ChromelyResponse();
            _mimeType = ResourceHandler.DefaultMimeType;
            _stream = Stream.Null;
        }

        /// <summary>
        /// The process request async.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
        {
            var scheme = _requestSchemeProvider?.GetScheme(request.Url);
            if (scheme is not null && scheme.UrlSchemeType == UrlSchemeType.LocalRequest)
            {
                _stream = Stream.Null;
                var uri = new Uri(request.Url);
                var path = uri.LocalPath;
                _mimeType = "application/json";

                bool isRequestAsync = _routeProvider.IsRouteAsync(path);
                if (isRequestAsync)
                {
                    ProcessRequestAsync(path);
                }
                else
                {
                    ProcessRequest(path);
                }
            }

            return CefReturnValue.ContinueAsync;

            #region Process Request

            void ProcessRequest(string path)
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        try
                        {
                            var response = new ChromelyResponse();
                            if (string.IsNullOrEmpty(path))
                            {
                                _chromelyResponse = _chromelyErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), path);
                            }
                            else
                            {
                                var parameters = request.Url.GetParameters();
                                var postData = GetPostData(request);

                                var jsonRequest = _dataTransferOptions.ConvertObjectToJson(request);
                                _chromelyResponse = _requestHandler.Execute(request.Identifier.ToString(), path, parameters, postData, jsonRequest);
                                string? jsonData = _dataTransferOptions.ConvertResponseToJson(_chromelyResponse.Data);

                                if (jsonData is not null)
                                {
                                    var content = Encoding.UTF8.GetBytes(jsonData);
                                    _stream = new MemoryStream();
                                    _stream.Write(content, 0, content.Length);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            _stream = Stream.Null;
                            var chromelyRequest = new ChromelyRequest() { Id = request.Identifier.ToString(), RouteUrl = request.Url };
                            _chromelyResponse = _chromelyErrorHandler.HandleError(chromelyRequest, exception);
                        }

                        if (_stream is null)
                        {
                            callback.Cancel();
                        }
                        else
                        {
                            SetResponseInfoOnSuccess();
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
                            var response = new ChromelyResponse();
                            if (string.IsNullOrEmpty(path))
                            {
                                _chromelyResponse = _chromelyErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), path);
                            }
                            else
                            {
                                var parameters = request.Url.GetParameters(request.ReferrerURL);
                                var postData = GetPostData(request);

                                var jsonRequest = _dataTransferOptions.ConvertObjectToJson(request);
                                _chromelyResponse = await _requestHandler.ExecuteAsync(request.Identifier.ToString(), path, parameters, postData, jsonRequest);
                                string? jsonData = _dataTransferOptions.ConvertResponseToJson(_chromelyResponse.Data);

                                if (jsonData is not null)
                                {
                                    var content = Encoding.UTF8.GetBytes(jsonData);
                                    _stream = new MemoryStream();
                                    _stream.Write(content, 0, content.Length);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            _stream = Stream.Null;
                            var chromelyRequest = new ChromelyRequest() { Id = request.Identifier.ToString(), RouteUrl = request.Url };
                            _chromelyResponse = _chromelyErrorHandler.HandleError(chromelyRequest, exception);
                        }

                        if (_stream is null)
                        {
                            callback.Cancel();
                        }
                        else
                        {
                            SetResponseInfoOnSuccess();
                            callback.Continue();
                        }
                    }
                });
            }

            #endregion
        }

        protected virtual void SetResponseInfoOnSuccess()
        {
            //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
            _stream.Position = 0;
            //Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
            ResponseLength = _stream.Length;
            MimeType = _mimeType;
            StatusCode = _chromelyResponse.Status;
            StatusText = _chromelyResponse.StatusText;
            Stream = _stream;
            MimeType = _mimeType;

            if (Headers is not null)
            {
                Headers.Add("Cache-Control", "private");
                Headers.Add("Access-Control-Allow-Methods", "GET,POST");
                Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                Headers.Add("Content-Type", "application/json; charset=utf-8");
            }
        }

        private static string GetPostData(CefRequest request)
        {
            var postDataElements = request?.PostData?.GetElements();
            if (postDataElements is null || (postDataElements.Length == 0))
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