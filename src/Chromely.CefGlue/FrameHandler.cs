// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.CefGlue.Browser.FrameHandlers;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class FrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private static CefBrowser _browser;

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFrame"/>.
        /// </returns>
        public static CefFrame GetMainFrame(this IChromelyContainer container)
        {
            return container.GetBrowser()?.GetMainFrame();
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="CefFrame"/>.
        /// </returns>
        public static CefFrame GetFrame(this IChromelyContainer container, string name)
        {
            return container.GetBrowser()?.GetFrame(name);
        }

        /// <summary>
        /// The get browser.
        /// </summary>
        /// <returns>
        /// The <see cref="CefBrowser"/>.
        /// </returns>
        public static CefBrowser GetBrowser(this IChromelyContainer container)
        {
            if (_browser != null)
            {
                return _browser;
            }

            var cefGlueFrameHandler = container.GetInstance<CefGlueFrameHandler>(typeof(CefGlueFrameHandler).FullName);
            if (cefGlueFrameHandler != null)
            {
                _browser = cefGlueFrameHandler.Browser;
            }

            return _browser;
        }
    }
}
