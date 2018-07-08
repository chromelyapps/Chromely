// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpContextMenuHandler.cs" company="Chromely">
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

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    using Chromely.Core;
    using Chromely.Core.Infrastructure;

    using global::CefSharp;

    /// <summary>
    /// The CefSharp context menu handler.
    /// </summary>
    public class CefSharpContextMenuHandler : IContextMenuHandler
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
        /// Initializes a new instance of the <see cref="CefSharpContextMenuHandler"/> class.
        /// </summary>
        public CefSharpContextMenuHandler()
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
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
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
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
             // To disable the menu then call clear
             model.Clear();

            // Removing existing menu item
            // Remove "View Source" option
            model.Remove(CefMenuCommand.ViewSource);

            if (this.debugging)
            {
                // Add new custom menu items
                model.AddItem((CefMenuCommand)ShowDevTools, "Show DevTools");
                model.AddItem((CefMenuCommand)CloseDevTools, "Close DevTools");
            }
        }

        /// <summary>
        /// The on context menu command.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
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
        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {

            if (this.debugging)
            {
                if ((int)commandId == ShowDevTools)
                {
                    browser.ShowDevTools();
                }

                if ((int)commandId == CloseDevTools)
                {
                    browser.CloseDevTools();
                }
            }

            return false;
        }

        /// <summary>
        /// The on context menu dismissed.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
        /// <param name="browser">
        /// The browser.
        /// </param>
        /// <param name="frame">
        /// The frame.
        /// </param>
        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }

        /// <summary>
        /// The run context menu.
        /// </summary>
        /// <param name="browserControl">
        /// The browser control.
        /// </param>
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
        bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}