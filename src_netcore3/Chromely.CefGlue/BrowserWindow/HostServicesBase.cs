using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chromely.CefGlue.BrowserWindow
{
    public abstract partial class HostBase 
    {
        #region IChromelyServiceProvider implementations

        /// <summary>
        /// The register service assembly.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void RegisterServiceAssembly(string filename)
        {
            _config?.ControllerAssemblies?.RegisterServiceAssembly(filename);
        }

        /// <summary>
        /// The register service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public void RegisterServiceAssembly(Assembly assembly)
        {
            _config?.ControllerAssemblies?.RegisterServiceAssembly(assembly);
        }

        /// <summary>
        /// The scan assemblies.
        /// </summary>
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
                    }
                }
            }
        }

        #endregion
    }
}
