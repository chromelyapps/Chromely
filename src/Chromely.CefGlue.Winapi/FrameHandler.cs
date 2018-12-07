// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Winapi
{
    using Chromely.CefGlue.Winapi.Browser.FrameHandlers;
    using Chromely.Core.Infrastructure;

    using Xilium.CefGlue;

    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class FrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private static CefBrowser browser;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        private static CefBrowser Browser
        {
            get
            {
                if (browser != null)
                {
                    return browser;
                }

                var cefGlueFrameHandler = IoC.GetInstance<CefGlueFrameHandler>(typeof(CefGlueFrameHandler).FullName);
                if (cefGlueFrameHandler != null)
                {
                    browser = cefGlueFrameHandler.Browser;
                }

                return browser;
            }
        }

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see cref="CefFrame"/>.
        /// </returns>
        public static CefFrame GetMainFrame()
        {
            return Browser?.GetMainFrame();
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="CefFrame"/>.
        /// </returns>
        public static CefFrame GetFrame(string name)
        {
            return Browser?.GetFrame(name);
        }

        /// <summary>
        /// The get browser.
        /// </summary>
        /// <returns>
        /// The <see cref="CefBrowser"/>.
        /// </returns>
        public static CefBrowser GetBrowser()
        {
            return Browser;
        }
    }
}
