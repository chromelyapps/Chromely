// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
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

        public CefBrowserApp(IChromelyConfiguration config, IChromelyRequestSchemeProvider requestSchemeProvider)
        {
            _config = config;
            _requestSchemeProvider = requestSchemeProvider;
            _renderProcessHandler = new DefaultRenderProcessHandler(_config);
            _browserProcessHandler = new DefaultBrowserProcessHandler(_config);
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
            if (schemes != null && schemes.Any())
            {
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
    }
}
