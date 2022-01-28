// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable IDE1006

namespace Chromely.Browser;

/// <summary>
/// Default CEF message router handler.
/// </summary>
/// <remarks>
/// Implements - https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage.md#markdown-header-generic-message-router
/// </remarks>
public class DefaultMessageRouterHandler : CefMessageRouterBrowserSide.Handler, IChromelyMessageRouter
{
    protected readonly IChromelyRouteProvider _routeProvider;
    protected readonly IChromelyRequestHandler _requestHandler;
    protected readonly IChromelyDataTransferOptions _dataTransferOptions;
    protected readonly IChromelyErrorHandler _chromelyErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultMessageRouterHandler"/>.
    /// </summary>
    /// <param name="routeProvider">Instance of <see cref="IChromelyRouteProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromelyRequestHandler"/>.</param>
    /// <param name="dataTransferOptions">Instance of <see cref="IChromelyDataTransferOptions"/>.</param>
    /// <param name="chromelyErrorHandler">Instance of <see cref="IChromelyErrorHandler"/>.</param>
    public DefaultMessageRouterHandler(IChromelyRouteProvider routeProvider, IChromelyRequestHandler requestHandler, IChromelyDataTransferOptions dataTransferOptions, IChromelyErrorHandler chromelyErrorHandler)
    {
        _routeProvider = routeProvider;
        _requestHandler = requestHandler;
        _dataTransferOptions = dataTransferOptions;
        _chromelyErrorHandler = chromelyErrorHandler;
    }

    /// <inheritdoc/>
    public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
    {
        request? requestData = null;

        try
        {
            requestData = JsonSerializer.Deserialize<request>(request, _dataTransferOptions.SerializerOptions as JsonSerializerOptions);

            if (requestData is not null)
            {
                var id = requestData.id ?? string.Empty;
                var path = requestData.url ?? string.Empty;

                bool isRequestAsync = _routeProvider.IsRouteAsync(path);

                if (isRequestAsync)
                {
                    Task.Run(async () =>
                    {
                        var parameters = requestData.parameters;
                        var postData = requestData.postData;

                        var response = await _requestHandler.ExecuteAsync(id, path, parameters, postData, request);
                        var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);

                        callback.Success(jsonResponse);
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        var parameters = requestData.parameters;
                        var postData = requestData.postData;

                        var response = _requestHandler.Execute(id, path, parameters, postData, request);
                        var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);

                        callback.Success(jsonResponse);
                    });
                }

                return true;
            }
        }
        catch (Exception exception)
        {
            var chromelyRequest = requestData?.ToRequest();
            if (chromelyRequest is null)
            {
                chromelyRequest = new ChromelyRequest();
            }

            var response = _chromelyErrorHandler.HandleError(chromelyRequest, exception);
            var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);
            callback.Failure(100, jsonResponse);
            return false;
        }

        callback.Failure(100, "Request is not valid.");
        return false;
    }

    /// <inheritdoc/>
    public override void OnQueryCanceled(CefBrowser browser, CefFrame frame, long queryId)
    {
    }

    private class request
    {
        public request()
        {
            id = Guid.NewGuid().ToString();
            url = string.Empty;
        }

        public string id { get; set; }
        public string url { get; set; }
        public IDictionary<string, object>? parameters { get; set; }
        public object? postData { get; set; }

        public IChromelyRequest ToRequest()
        {
            return new ChromelyRequest(id, url, parameters, postData, null);
        }
    }
}
