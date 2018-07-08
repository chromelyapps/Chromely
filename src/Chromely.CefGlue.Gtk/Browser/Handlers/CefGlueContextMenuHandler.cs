// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueContextMenuHandler.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.Browser.Handlers
{
    using System;

    using Chromely.Core;
    using Chromely.Core.Infrastructure;

    using Xilium.CefGlue;

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
            var config = IoC.GetInstance(typeof(ChromelyConfiguration), typeof(ChromelyConfiguration).FullName);
            if (config is ChromelyConfiguration)
            {
                var chromelyConfiguration = (ChromelyConfiguration)config;
                this.debugging = chromelyConfiguration.DebuggingMode;
            }
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

            if (this.debugging)
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
            if (this.debugging)
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