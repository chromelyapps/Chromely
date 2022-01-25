// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

public partial class WindowController
{
    protected virtual void RegisterDefaultSchemeHandlers()
    {
        if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
        {
            ActionTask.PostTask(CefThreadId.UI, RegisterDefaultSchemeHandlers);
            return;
        }

        if (_handlersResolver is null)
            return;

        // Register default resource/reguest handlers
        IDictionary<UrlSchemeType, Type> urlTypesMapper = new Dictionary<UrlSchemeType, Type>
            {
                { UrlSchemeType.LocalResource, typeof(IDefaultResourceCustomHandler) },
                { UrlSchemeType.FolderResource, typeof(IDefaultResourceCustomHandler) },
                { UrlSchemeType.AssemblyResource, typeof(IDefaultAssemblyResourceCustomHandler) },
                { UrlSchemeType.LocalRequest, typeof(IDefaultRequestCustomHandler) },
                { UrlSchemeType.Owin, typeof(IDefaultOwinCustomHandler) },
                { UrlSchemeType.ExternalRequest, typeof(IDefaultExernalRequestCustomHandler) }
            };

        foreach (var urlType in urlTypesMapper)
        {
            var handler = _handlersResolver.GetDefaultHandler(typeof(CefSchemeHandlerFactory), urlType.Value);

            if (handler is CefSchemeHandlerFactory schemeHandlerFactory)
            {
                var schemes = _config?.UrlSchemes.GetSchemesByType(urlType.Key);
                if (schemes is not null && schemes.Any())
                {
                    foreach (var item in schemes)
                    {
                        _requestSchemeProvider.Add(item);
                        CefRuntime.RegisterSchemeHandlerFactory(item.Scheme, item.Host, schemeHandlerFactory);
                    }
                }
            }
        }
    }

    protected virtual void RegisterCustomSchemeHandlers()
    {
        if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
        {
            ActionTask.PostTask(CefThreadId.UI, RegisterCustomSchemeHandlers);
            return;
        }

        // Register custom request handlers
        var schemeHandlerList = _handlersResolver?.Invoke(typeof(IChromelySchemeHandler));
        if (schemeHandlerList is not null && schemeHandlerList.Any())
        {
            foreach (var schemeHandlerObj in schemeHandlerList)
            {
                if (schemeHandlerObj is not IChromelySchemeHandler schemehandler ||
                    schemehandler.Scheme is null ||
                    !schemehandler.Scheme.IsValidSchemeAndHost)
                    continue;

                _requestSchemeProvider.Add(schemehandler.Scheme);
                var schemeHandlerFactory = schemehandler.HandlerFactory as CefSchemeHandlerFactory;
                if (schemeHandlerFactory is not null)
                {
                    CefRuntime.RegisterSchemeHandlerFactory(schemehandler.Scheme.Scheme, schemehandler.Scheme.Host, schemeHandlerFactory);
                }
            }
        }
    }

    private void ResolveHandlers()
    {
        var schemes = _config?.UrlSchemes;
        if (schemes is not null && schemes.Any())
        {
            foreach (var item in schemes)
            {
                if (item.UrlSchemeType == UrlSchemeType.LocalResource ||
                    item.UrlSchemeType == UrlSchemeType.FolderResource ||
                    item.UrlSchemeType == UrlSchemeType.AssemblyResource ||
                    item.UrlSchemeType == UrlSchemeType.LocalRequest ||
                    item.UrlSchemeType == UrlSchemeType.ExternalRequest)
                {
                    _requestSchemeProvider.Add(item);
                }
            }
        }

        var schemeHandlerList = _handlersResolver?.Invoke(typeof(IChromelySchemeHandler));
        if (schemeHandlerList is not null && schemeHandlerList.Any())
        {
            foreach (var schemeHandlerObj in schemeHandlerList)
            {
                if (schemeHandlerObj is not IChromelySchemeHandler schemehandler ||
                    schemehandler.Scheme is null ||
                    !schemehandler.Scheme.IsValidSchemeAndHost)
                    continue;

                _requestSchemeProvider.Add(schemehandler.Scheme);
            }
        }
    }
}