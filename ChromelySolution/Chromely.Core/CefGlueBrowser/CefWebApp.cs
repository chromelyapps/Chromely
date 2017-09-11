#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Core.CefGlueBrowser
{
    using Xilium.CefGlue;

    public sealed class CefWebApp : CefApp
    {
        private string m_scheme;
        public CefWebApp(string scheme)
        {
            m_scheme = scheme;
        }

        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            registrar.AddCustomScheme(m_scheme, false, false, false, false, true, true);
        }

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            ;
        }
    }
}
