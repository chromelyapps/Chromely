// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueContextMenuHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue context menu handler.
    /// </summary>
    public class CefGlueContextMenuHandler : CefContextMenuHandler
    {
        /// <summary>
        /// The show dev tools.
        /// </summary>
        private const int ShowDevTools = 26501;

        /// <summary>
        /// The close dev tools.
        /// </summary>
        private const int CloseDevTools = 26502;

        /// <summary>
        /// The debugging.
        /// </summary>
        private readonly bool debugging;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueContextMenuHandler"/> class.
        /// </summary>
        public CefGlueContextMenuHandler()
        {
            debugging = ChromelyConfiguration.Instance.DebuggingMode;
        }

        /// <summary>
        /// The on before context menu.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
        {
            // To disable the menu then call clear
            model.Clear();

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

        /// <summary>
        /// The run context menu.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
        {
            return false;
        }

        /// <summary>
        /// The on context menu command.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="commandId">
        /// The command id.
        /// </param>
        /// <param name="eventFlags">
        /// The event flags.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags)
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

            return false;
        }

        /// <summary>
        /// The on context menu dismissed.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        protected override void OnContextMenuDismissed(CefBrowser browser, CefFrame frame)
        {
        }

        /// <summary>
        /// The dev tools web client.
        /// </summary>
        private class DevToolsWebClient : CefClient
        {
        }
    }
}