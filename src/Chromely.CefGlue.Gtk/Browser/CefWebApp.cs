#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Gtk.Browser
{
    using Chromely.CefGlue.Gtk.Browser.Handlers;
    using Chromely.Core.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using Xilium.CefGlue;

    internal sealed class CefWebApp : CefApp
    {
        private CefRenderProcessHandler m_renderProcessHandler = new CefGlueRenderProcessHandler();

        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            IEnumerable<object> schemeHandlerObjs = IoC.GetAllInstances(typeof(ChromelySchemeHandler));
            if (schemeHandlerObjs != null)
            {
                var schemeHandlers = schemeHandlerObjs.ToList();

                foreach (var item in schemeHandlers)
                {
                    if (item is ChromelySchemeHandler)
                    {
                        ChromelySchemeHandler handler = (ChromelySchemeHandler)item;
                        if (handler.HandlerFactory is CefSchemeHandlerFactory)
                        {
                            registrar.AddCustomScheme(handler.SchemeName, false, false, false, false, true, true);
                         }
                    }
                }
            }
        }

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            ;
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return m_renderProcessHandler;
        }
    }
}
