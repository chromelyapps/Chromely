// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

internal class CefBrowserApp : CefApp
{
    private readonly CefRenderProcessHandler _renderProcessHandler;
    private readonly CefBrowserProcessHandler _browserProcessHandler;
    private readonly IChromelyConfiguration _config;
    private readonly IChromelyRequestSchemeProvider _requestSchemeProvider;
    private readonly ChromelyHandlersResolver _handlersResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="CefBrowserApp"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="requestSchemeProvider">Instance of <see cref="IChromelyRequestSchemeProvider"/>.</param>
    /// <param name="handlersResolver">Instance of <see cref="ChromelyHandlersResolver"/>.</param>
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
        var schemeExes = new List<UrlSchemeEx>();
        if (schemes is not null && schemes.Any())
        {
            foreach (var item in schemes)
            {
                schemeExes.Add(new UrlSchemeEx(item));
            }
        }

        var schemeHandlerList = _handlersResolver?.Invoke(typeof(IChromelySchemeHandler));
        if (schemeHandlerList is not null && schemeHandlerList.Any())
        {
            foreach (var handler in schemeHandlerList)
            {
                if (handler is IChromelySchemeHandler schemeHandler)
                {
                    if (schemeHandler?.Scheme is not null && schemeHandler.Scheme.IsValidSchemeAndHost)
                    {
                        // add if not already added
                        var firstOrDefault = schemeExes.FirstOrDefault(x => x.ValidSchemeHost &&
                                                                      x.Scheme.ToLower().Equals(schemeHandler.Scheme.Scheme.ToLower()) &&
                                                                      x.Host.ToLower().Equals(schemeHandler.Scheme.Host.ToLower()));
                        if (firstOrDefault is null)
                        {
                            schemeExes.Add(new UrlSchemeEx(schemeHandler.Scheme, schemeHandler.IsCorsEnabled, schemeHandler.IsSecure));
                        }
                    }
                }
            }
        }

        var schemeOptions = CefSchemeOptions.Local | CefSchemeOptions.Standard | CefSchemeOptions.CorsEnabled | CefSchemeOptions.Secure | CefSchemeOptions.CorsEnabled | CefSchemeOptions.FetchEnabled;

        foreach (var scheme in schemeExes)
        {
            bool isStandardScheme = UrlScheme.IsStandardScheme(scheme.Scheme);
            if (!isStandardScheme)
            {
                if (!scheme.IsCorsEnabled)
                {
                    schemeOptions &= ~CefSchemeOptions.CorsEnabled;
                }

                if (!scheme.IsSecure)
                {
                    schemeOptions &= ~CefSchemeOptions.Secure;
                }

                registrar.AddCustomScheme(scheme.Scheme, schemeOptions);
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
        if (_config is not null)
        {
            if (_config.CommandLineArgs is not null)
            {
                foreach (var commandArg in _config.CommandLineArgs)
                {
                    commandLine.AppendSwitch(commandArg.Key ?? string.Empty, commandArg.Value);
                }
            }

            if (_config.CommandLineOptions is not null)
            {
                foreach (var commmandOption in _config.CommandLineOptions)
                {
                    commandLine.AppendSwitch(commmandOption ?? string.Empty);
                }
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

    private class UrlSchemeEx
    {
        private readonly UrlScheme _urlScheme;

        public UrlSchemeEx(UrlScheme urlScheme, bool isCorsEnabled = true, bool isSecure = false)
        {
            _urlScheme = urlScheme;
            IsCorsEnabled = isCorsEnabled;
            IsSecure = isSecure;
        }

        public string Scheme
        {
            get
            {
                if (_urlScheme is null)
                    return string.Empty;

                return _urlScheme.Scheme;
            }
        }
        public string Host
        {
            get
            {
                if (_urlScheme is null)
                    return string.Empty;

                return _urlScheme.Host;
            }
        }

        public bool ValidSchemeHost
        {
            get
            {
                if (_urlScheme is null)
                    return false;

                return _urlScheme.IsValidSchemeAndHost;
            }
        }

        public bool IsCorsEnabled { get; }
        public bool IsSecure { get; }
    }
}