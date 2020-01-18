using Chromely.CefGlue.Browser.Handlers;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;

namespace Chromely.CefGlue.BrowserWindow
{
    public abstract partial class HostBase 
    {
        #region IChromelyServiceProvider implementations

        public void RegisterServiceAssembly(string filename)
        {
            _config?.ControllerAssemblies?.RegisterServiceAssembly(filename);
        }

        public void RegisterServiceAssembly(Assembly assembly)
        {
            _config?.ControllerAssemblies?.RegisterServiceAssembly(assembly);
        }

        public void ScanAssemblies()
        {
            if ((_config?.ControllerAssemblies == null) || (_config?.ControllerAssemblies.Count == 0))
            {
                return;
            }

            foreach (var assembly in _config?.ControllerAssemblies)
            {
                if (!assembly.IsScanned)
                {
                    var scanner = new RouteScanner(assembly.Assembly, _container);
                    var scanResult = scanner.Scan();
                    var actionRoutes = scanResult?.Item1;
                    var commandRoutes = scanResult?.Item2;
                    ServiceRouteProvider.RegisterActionRoutes(_container, actionRoutes);
                    ServiceRouteProvider.RegisterCommnandRoutes(_container, commandRoutes);

                    assembly.IsScanned = true;
                }
            }
        }

        public void RegisterRoutes()
        {
            // Get all Controllers
            var controllerObjs = _container.GetAllInstances(typeof(ChromelyController)).ToList();
            if (controllerObjs != null && controllerObjs.Any())
            {
                foreach (var obj in controllerObjs)
                {
                    var controller = obj as ChromelyController;
                    if (controller != null)
                    {

                        ServiceRouteProvider.RegisterActionRoutes(_container, controller.ActionRouteDictionary);
                        ServiceRouteProvider.RegisterCommnandRoutes(_container, controller.CommandRouteDictionary);

                        var actionRouteDictionary = new Dictionary<string, ActionRoute>();
                        var commandRouteDictionary = new Dictionary<string, CommandRoute>();

                        var controllerFactory = new ChromelyControllerFactory(_container);

                        // Add Http Attributes
                        var httpAttributeRoutes = controllerFactory.GetHttpAttributeRoutes(controller);
                        if ((httpAttributeRoutes != null) && httpAttributeRoutes.Any())
                        {
                            foreach (var item in httpAttributeRoutes)
                            {
                                if (!actionRouteDictionary.ContainsKey(item.Key))
                                {
                                    actionRouteDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }

                        // Add Custom Attributes
                        var customAttributeRoutes = controllerFactory.GetCommandAttributeRoutes(controller);
                        if ((customAttributeRoutes != null) && customAttributeRoutes.Any())
                        {
                            foreach (var item in customAttributeRoutes)
                            {
                                if (!commandRouteDictionary.ContainsKey(item.Key))
                                {
                                    commandRouteDictionary.Add(item.Key, item.Value);
                                }
                            }
                        }


                        ServiceRouteProvider.RegisterActionRoutes(_container, actionRouteDictionary);
                        ServiceRouteProvider.RegisterCommnandRoutes(_container, commandRouteDictionary);
                    }
                }
            }
        }

        #endregion
        #region CEF Network Service Handlers

        /// <summary>
        /// The register resource handlers.
        /// </summary>
        private void RegisterResourceHandlers()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                PostTask(CefThreadId.UI, RegisterResourceHandlers);
                return;
            }

            // Register resource handlers
            var resourceSchemes = _config?.UrlSchemes.GetAllResouceSchemes();
            if (resourceSchemes != null && resourceSchemes.Any())
            {
                foreach (var item in resourceSchemes)
                {
                    bool isDefault = true;
                    if (!string.IsNullOrWhiteSpace(item.Name))
                    {
                        var resourceObj = _container.GetInstance(typeof(IChromelyResourceHandlerFactory), item.Name);
                        var resourceHandlerFactory = resourceObj as CefSchemeHandlerFactory;
                        if (resourceHandlerFactory != null)
                        {
                            isDefault = false;
                            CefRuntime.RegisterSchemeHandlerFactory(item.Scheme, item.Host, resourceHandlerFactory);
                        }
                    }

                    if (isDefault)
                    {
                        CefRuntime.RegisterSchemeHandlerFactory(item.Scheme, item.Host, new CefGlueResourceSchemeHandlerFactory());
                    }
                }
            }
        }

        /// <summary>
        /// The register scheme handlers.
        /// </summary>
        private void RegisterSchemeHandlers()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                PostTask(CefThreadId.UI, RegisterSchemeHandlers);
                return;
            }

            // Register scheme handlers
            var schemeSchemes = _config?.UrlSchemes.GetAllCustomSchemes();
            if (schemeSchemes != null && schemeSchemes.Any())
            {
                foreach (var item in schemeSchemes)
                {
                    bool isDefault = true;
                    if (!string.IsNullOrWhiteSpace(item.Name))
                    {
                        var schemeObj = _container.GetInstance(typeof(IChromelySchemeHandlerFactory), item.Name);
                        var schemeHandlerFactory = schemeObj as CefSchemeHandlerFactory;
                        if (schemeHandlerFactory != null)
                        {
                            isDefault = false;
                            CefRuntime.RegisterSchemeHandlerFactory(item.Scheme, item.Host, schemeHandlerFactory);
                        }
                    }

                    if (isDefault)
                    {
                        CefRuntime.RegisterSchemeHandlerFactory(item.Scheme, item.Host, new CefGlueHttpSchemeHandlerFactory(_config, _requestTaskRunner));
                    }
                }
            }
        }

        /// <summary>
        /// The register message routers.
        /// </summary>
        private void RegisterMessageRouters()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                PostTask(CefThreadId.UI, RegisterMessageRouters);
                return;
            }

            BrowserMessageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());

            // Register message router handlers
            var messageRouterHandlers = _container.GetAllInstances(typeof(IChromelyMessageRouter));
            if (messageRouterHandlers == null && messageRouterHandlers.Any())
            {
                foreach (var handler in messageRouterHandlers)
                {
                    var router = handler as CefMessageRouterBrowserSide.Handler;
                    if (router != null)
                    {
                        BrowserMessageRouter.AddHandler(router);
                    }
                }
            }
            else
            {
                BrowserMessageRouter.AddHandler(new CefGlueMessageRouterHandler(_requestTaskRunner));
            }
        }

        #endregion

    }
}
