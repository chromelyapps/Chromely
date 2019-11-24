using System;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    public class CefGlueContextMenuHandler : CefContextMenuHandler
    {
        private readonly IChromelyConfiguration _config;

        private const int ShowDevTools = 26501;

        private const int CloseDevTools = 26502;

        private readonly bool debugging;

        public CefGlueContextMenuHandler(IChromelyConfiguration config)
        {
            _config = config;
            debugging = _config.DebuggingMode;
        }

        protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
        {
            // To disable the menu then call clear
            model.Clear();

            if (_config.Platform == ChromelyPlatform.Windows)
            {
                // Removing existing menu item
                // Remove "View Source" option
                model.Remove((int)CefMenuId.ViewSource);

                if (debugging)
                {
                    // Add new custom menu items
                    model.AddItem((int)((CefMenuId)ShowDevTools), "Show DevTools");
                    model.AddItem((int)((CefMenuId)CloseDevTools), "Close DevTools");
                }
            }
        }

        protected override bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
        {
            return false;
        }

        protected override bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags)
        {
            if (_config.Platform == ChromelyPlatform.Windows)
            {
                if (debugging)
                {
                    if (commandId == ShowDevTools)
                    {
                        var host = browser.GetHost();
                        var wi = CefWindowInfo.Create();
                        wi.SetAsPopup(IntPtr.Zero, "DevTools");
                        host.ShowDevTools(wi, new DevToolsWebClient(), new CefBrowserSettings(), new CefPoint(0, 0));
                    }

                    if (commandId == CloseDevTools)
                    {
                        browser.GetHost().CloseDevTools();
                    }
                }
            }

            return false;
        }

        protected override void OnContextMenuDismissed(CefBrowser browser, CefFrame frame)
        {
        }

        private class DevToolsWebClient : CefClient
        {
        }
    }
}