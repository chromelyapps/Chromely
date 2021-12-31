// Copyright (c) Alex Maitland. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Chromely.Owin;

/// <summary>
/// Loosly based on https://github.com/eVisionSoftware/Harley/blob/master/src/Harley.UI/Owin/OwinSchemeHandlerFactory.cs
/// New instance is instanciated for every request
/// </summary>
public class OwinSchemeHandler : ResourceHandler
{
    protected readonly IOwinPipeline _owinPipeline;
    protected readonly IChromelyErrorHandler _errorHandler;
   
    public OwinSchemeHandler(IOwinPipeline owinPipeline, IChromelyErrorHandler errorHandler)
    {
        _owinPipeline = owinPipeline;
        _errorHandler = errorHandler;
    }

    /// <summary>
    /// Read the request, then process it through the OWIN pipeline
    /// then populate the response properties.
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="callback">callback</param>
    /// <returns>always returns true as we'll handle all requests this handler is registered for.</returns>
    public override CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
    {
        var requestBody = Stream.Null;
        if (request.Method == "POST")
        {
            using (var postData = request.PostData)
            {
                if (postData != null)
                {
                    var postDataElements = postData.GetElements();
                    var firstPostDataElement = postDataElements.First();
                    var bytes = firstPostDataElement.GetBytes();
                    requestBody = new MemoryStream(bytes, 0, bytes.Length);
                }
            }
        }

        var uri = new Uri(request.Url);
        var requestHeaders = request.GetHeaderMap();
        //Add Host header as per http://owin.org/html/owin.html#5-2-hostname
        requestHeaders.Add("Host", uri.Host + (uri.Port > 0 ? (":" + uri.Port) : ""));

        Task.Run(async () =>
        {
            IChromelyResource chromelyResource = new ChromelyResource();

            try
            {
                // Call into the OWIN pipeline
                var owinRequest = new ResourceRequest(request.Url, request.Method, requestHeaders, requestBody);
                var owinResponse = await RequestInterceptor.ProcessRequest(_owinPipeline.AppFunc, owinRequest);

                chromelyResource =  new ChromelyResource()
                {
                    Content = owinResponse.Stream as MemoryStream,
                    MimeType = owinResponse.Headers.GetMimeType(),
                    StatusCode = (HttpStatusCode)owinResponse.StatusCode,
                    StatusText = owinResponse.ReasonPhrase,
                    Headers = owinResponse.Headers
                };

                if (chromelyResource.StatusCode.IsClientErrorCode() || chromelyResource.StatusCode.IsServerErrorCode())
                {
                    chromelyResource = await _errorHandler.HandleErrorAsync(request.Url, chromelyResource, null);
                }
            }
            catch (Exception exception)
            {
                chromelyResource = await _errorHandler.HandleErrorAsync(request.Url, chromelyResource, exception);
            }

            //Populate the response properties
            Stream = chromelyResource.Content;
            ResponseLength = (chromelyResource.Content ==  null ) ? 0 : chromelyResource.Content.Length;
            StatusCode = (int)chromelyResource.StatusCode;
            MimeType = chromelyResource.MimeType;

            foreach (var header in chromelyResource.Headers)
            {
                //It's possible for headers to have multiple values
                foreach (var val in header.Value)
                {
                    Headers.Add(header.Key, val);
                }
            }

            //Once we've finished populating the properties we execute the callback
            //Callback wraps an unmanaged resource, so let's explicitly Dispose when we're done    
            using (callback)
            {
                callback.Continue();
            }
        });

        return CefReturnValue.ContinueAsync;
    }
}