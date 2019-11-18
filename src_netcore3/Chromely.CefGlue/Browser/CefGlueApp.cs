// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueApp.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.IO;
using Chromely.CefGlue.Browser.Handlers;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser
{
    public class CefGlueApp : CefApp
    {
        private readonly CefRenderProcessHandler _renderProcessHandler;

        private readonly CefBrowserProcessHandler _browserProcessHandler;
        
        private readonly IChromelyConfiguration _config;

        public CefGlueApp(IChromelyConfiguration config)
        {
            _config = config;
             _renderProcessHandler = new CefGlueRenderProcessHandler(_config);
            _browserProcessHandler = new CefGlueBrowserProcessHandler(_config);
        }

        /// <summary>
        /// The on register custom schemes.
        /// </summary>
        /// <param name="registrar">
        /// The registrar.
        /// </param>
        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            var resourceSchemes = _config?.UrlSchemes?.GetAllResouceSchemes();
            if (resourceSchemes != null)
            {
                foreach (var item in resourceSchemes)
                {
                    bool isStandardScheme = UrlScheme.IsStandardScheme(item.Scheme);
                    if (!isStandardScheme)
                    {
                        var option = CefSchemeOptions.Local | CefSchemeOptions.CorsEnabled;
                        registrar.AddCustomScheme(item.Scheme, option);
                    }
                }
            }

            var customSchemes = _config?.UrlSchemes?.GetAllCustomSchemes();
            if (customSchemes != null)
            {
                foreach (var item in customSchemes)
                {
                    bool isStandardScheme = UrlScheme.IsStandardScheme(item.Scheme);
                    if (!isStandardScheme)
                    {
                        var option = CefSchemeOptions.Local | CefSchemeOptions.CorsEnabled;
                        registrar.AddCustomScheme(item.Scheme, option);
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
                    commandLine.AppendSwitch(commandArg.Item1 ?? string.Empty, commandArg.Item2);
                }
            }

            if (_config?.CommandLineOptions != null)
            {
                foreach (var commmandOption in _config?.CommandLineOptions)
                {
                    commandLine.AppendSwitch(commmandOption ?? string.Empty);
                }
            }

            // Currently on linux platform location of locales and pack files are determined
            // incorrectly (relative to main module instead of libcef.so module).
            // Once issue http://code.google.com/p/chromiumembedded/issues/detail?id=668 will be resolved
            // this code can be removed.
            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                if (string.IsNullOrEmpty(processType) || processType.Contains("zygote"))
                    commandLine.AppendSwitch("no-zygote");

                commandLine.AppendArgument("--disable-gpu");
                commandLine.AppendArgument("--disable-software-rasterizer");

                commandLine.AppendSwitch("disable-gpu", "1");
                commandLine.AppendSwitch("disable-software-rasterizer", "1");

                commandLine.AppendSwitch("resources-dir-path", _config.AppExeLocation);
                commandLine.AppendSwitch("locales-dir-path", Path.Combine(_config.AppExeLocation, "locales"));
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
