// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using System.Collections.Generic;
using System.Linq;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    internal class CefBrowserApp : CefApp
    {
        private readonly CefRenderProcessHandler _renderProcessHandler;
        private readonly CefBrowserProcessHandler _browserProcessHandler;
        private readonly IChromelyConfiguration _config;
        private readonly IChromelyRequestSchemeProvider _requestSchemeProvider;
        private readonly ChromelyHandlersResolver _handlersResolver;

        public CefBrowserApp(IChromelyConfiguration config, IChromelyRequestSchemeProvider requestSchemeProvider, ChromelyHandlersResolver handlersResolver)
        {
            _config = config;
            _requestSchemeProvider = requestSchemeProvider;
            _handlersResolver = handlersResolver;
            _renderProcessHandler = RenderProcessHandler;
            _browserProcessHandler = BrowserProcessHandler;
        }

        /// <summary>
        /// The on register custom schemes.
        /// </summary>
        /// <param name="registrar">
        /// The registrar.
        /// </param>
        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            var schemes = _requestSchemeProvider?.GetAllSchemes();
            if (schemes == null)
            {
                schemes = new List<UrlScheme>();
            }

            var schemeHandlerList = _handlersResolver?.Invoke(typeof(IChromelySchemeHandler));
            if (schemeHandlerList != null && schemeHandlerList.Any())
            {
                foreach (var handler in schemeHandlerList)
                {
                    if (handler is IChromelySchemeHandler schemeHandler)
                    {
                        if (schemeHandler?.Scheme != null && schemeHandler.Scheme.ValidSchemeHost)
                        {
                            // add if not already added
                            var firstOrDefault = schemes.FirstOrDefault(x => x.ValidSchemeHost &&
                                                                          x.Scheme.ToLower().Equals(schemeHandler.Scheme.Scheme.ToLower()) &&
                                                                          x.Host.ToLower().Equals(schemeHandler.Scheme.Host.ToLower()));
                            if (firstOrDefault == null)
                            {
                                schemes.Add(schemeHandler.Scheme);
                            }
                        }
                    }
                }
            }

            foreach (var scheme in schemes)
            {
                bool isStandardScheme = UrlScheme.IsStandardScheme(scheme.Scheme);
                if (!isStandardScheme)
                {
                    var option = CefSchemeOptions.Local | CefSchemeOptions.CorsEnabled;
                    registrar.AddCustomScheme(scheme.Scheme, option);
                }
            }
        }

        /// <summary>
        /// The on before command line processing.
        /// </summary>
        /// <param name="processType">
        /// The process type.
        /// </param>
        /// <param name="commandLine">
        /// The command line.
        /// </param>
        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            // Get all custom command line argument switches
            if (_config?.CommandLineArgs != null)
            {
                foreach (var commandArg in _config.CommandLineArgs)
                {
                    commandLine.AppendSwitch(commandArg.Key ?? string.Empty, commandArg.Value);
                }
            }

            if (_config?.CommandLineOptions != null)
            {
                foreach (var commmandOption in _config?.CommandLineOptions)
                {
                    commandLine.AppendSwitch(commmandOption ?? string.Empty);
                }
            }
        }

        /// <summary>
        /// The get render process handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefRenderProcessHandler"/>.
        /// </returns>
        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }

        /// <summary>
        /// The get browser process handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefBrowserProcessHandler"/>.
        /// </returns>
        protected override CefBrowserProcessHandler GetBrowserProcessHandler()
        {
            return _browserProcessHandler;
        }

        private CefRenderProcessHandler RenderProcessHandler
        {
            get
            {
                var handler = _handlersResolver.GetCustomOrDefaultHandler(typeof(CefRenderProcessHandler));
                if (handler is CefRenderProcessHandler renderProcessHandler)
                {
                    return renderProcessHandler;
                }

                return new DefaultRenderProcessHandler(_config);
            }
        }

        private CefBrowserProcessHandler BrowserProcessHandler
        {
            get
            {
                var handler = _handlersResolver.GetCustomOrDefaultHandler(typeof(CefBrowserProcessHandler));
                if (handler is CefBrowserProcessHandler browserProcesHandler)
                {
                    return browserProcesHandler;
                }

                return new DefaultBrowserProcessHandler(_config);
            }
        }
    }
}
