
namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using System;
    using Xilium.CefGlue;

    internal sealed class CefGlueContextMenuHandler : CefContextMenuHandler
    {
        private const int ShowDevTools = 26501;
        private const int CloseDevTools = 26502;

        protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
        {
            //To disable the menu then call clear
            model.Clear();

            //Removing existing menu item
            // Remove "View Source" option
            bool removed = model.Remove((int)CefMenuId.ViewSource);

            //Add new custom menu items
            model.AddItem((int)((CefMenuId)ShowDevTools), "Show DevTools");
            model.AddItem((int)((CefMenuId)CloseDevTools), "Close DevTools");
        }

        protected override bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
        {
            return false;
        }

        protected override bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags)
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