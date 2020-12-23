// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using Chromely.Browser;
using Chromely.Core;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely
{
    public partial class WindowController
    {
        protected virtual void RegisterDefaultSchemeHandlers()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                ActionTask.PostTask(CefThreadId.UI, RegisterDefaultSchemeHandlers);
                return;
            }

            if (_handlersResolver == null)
                return;

            // Register default resource/reguest handlers
            IDictionary<UrlSchemeType, Type> urlTypesMapper = new Dictionary<UrlSchemeType, Type>();
            urlTypesMapper.Add(UrlSchemeType.Resource, typeof(IDefaultResourceCustomHandler));
            urlTypesMapper.Add(UrlSchemeType.AssemblyResource, typeof(IDefaultAssemblyResourceCustomHandler));
            urlTypesMapper.Add(UrlSchemeType.LocalRequest, typeof(IDefaultRequestCustomHandler));
            urlTypesMapper.Add(UrlSchemeType.ExternalRequest, typeof(IDefaultExernalRequestCustomHandler));

            foreach (var urlType in urlTypesMapper)
            {
                var handler = _handlersResolver.GetDefaultHandler(typeof(CefSchemeHandlerFactory), urlType.Value);

                if (handler is CefSchemeHandlerFactory schemeHandlerFactory)
                {
                    var schemes = _config?.UrlSchemes.GetSchemesByType(urlType.Key);
                    if (schemes != null && schemes.Any())
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
            if (schemeHandlerList != null && schemeHandlerList.Any())
            {
                foreach (var schemeHandlerObj in schemeHandlerList)
                {
                    var schemehandler = schemeHandlerObj as IChromelySchemeHandler;
                    if (schemehandler == null ||
                        schemehandler.Scheme == null ||
                        !schemehandler.Scheme.ValidSchemeHost)
                        continue;

                    _requestSchemeProvider.Add(schemehandler.Scheme);
                    var schemeHandlerFactory = schemehandler.HandlerFactory as CefSchemeHandlerFactory;
                    CefRuntime.RegisterSchemeHandlerFactory(schemehandler.Scheme.Scheme, schemehandler.Scheme.Host, schemeHandlerFactory);
                }
            }
        }

        private void ResolveHandlers()
        {
            var schemes = _config?.UrlSchemes;
            if (schemes != null && schemes.Any())
            {
                foreach (var item in schemes)
                {
                    if (item.UrlSchemeType == UrlSchemeType.Resource ||
                        item.UrlSchemeType == UrlSchemeType.AssemblyResource ||
                        item.UrlSchemeType == UrlSchemeType.LocalRequest ||
                        item.UrlSchemeType == UrlSchemeType.ExternalRequest)
                    {
                        _requestSchemeProvider.Add(item);
                    }
                }
            }

            var schemeHandlerList = _handlersResolver?.Invoke(typeof(IChromelySchemeHandler));
            if (schemeHandlerList != null && schemeHandlerList.Any())
            {
                foreach (var schemeHandlerObj in schemeHandlerList)
                {
                    var schemehandler = schemeHandlerObj as IChromelySchemeHandler;
                    if (schemehandler == null ||
                        schemehandler.Scheme == null ||
                        !schemehandler.Scheme.ValidSchemeHost)
                        continue;

                    _requestSchemeProvider.Add(schemehandler.Scheme);
                }
            }
        }
    }
}
