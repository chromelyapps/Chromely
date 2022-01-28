// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default implementation of <see cref="CefContextMenuHandler"/>.
/// </summary>
public class DefaultContextMenuHandler : CefContextMenuHandler
{
    protected readonly IChromelyConfiguration _config;
    protected readonly bool debugging;

    protected const int ShowDevTools = 26501;
    protected const int CloseDevTools = 26502;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultContextMenuHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    public DefaultContextMenuHandler(IChromelyConfiguration config)
    {
        _config = config;
        debugging = _config.DebuggingMode;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
    {
        return false;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override void OnContextMenuDismissed(CefBrowser browser, CefFrame frame)
    {
    }

    private class DevToolsWebClient : CefClient
    {
    }
}