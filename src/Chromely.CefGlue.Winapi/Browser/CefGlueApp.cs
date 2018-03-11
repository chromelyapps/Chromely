#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Chromely.CefGlue.Winapi.Browser.Handlers;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    public class CefGlueApp : CefApp
    {
        private CefRenderProcessHandler m_renderProcessHandler = new CefGlueRenderProcessHandler();
        public ChromelyConfiguration m_hostConfig;

        public CefGlueApp(ChromelyConfiguration hostConfig)
        {
            m_hostConfig = hostConfig;
        }

        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            object[] schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));
            if (schemeHandlerObjs != null)
            {
                var schemeHandlers = schemeHandlerObjs.ToList();

                foreach (var item in schemeHandlers)
                {
                    if (item is ChromelySchemeHandler)
                    {
                        ChromelySchemeHandler handler = (ChromelySchemeHandler)item;
                        registrar.AddCustomScheme(handler.SchemeName, false, false, false, false, true, true);
                    }
                }
            }
        }

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            // Get all custom command line argument switches
            if ((m_hostConfig != null) && (m_hostConfig.CommandLineArgs != null))
            {
                foreach (var commandArg in m_hostConfig.CommandLineArgs)
                {
                    commandLine.AppendSwitch(commandArg.Key, commandArg.Value);
                }
            }

            // Currently on linux platform location of locales and pack files are determined
            // incorrectly (relative to main module instead of libcef.so module).
            // Once issue http://code.google.com/p/chromiumembedded/issues/detail?id=668 will be resolved
            // this code can be removed.
                if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

                commandLine.AppendSwitch("resources-dir-path", path);
                commandLine.AppendSwitch("locales-dir-path", Path.Combine(path, "locales"));
            }
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return m_renderProcessHandler;
        }
    }
}
