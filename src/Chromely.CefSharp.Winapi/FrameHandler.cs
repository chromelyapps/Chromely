// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi
{
    using global::CefSharp;
    using Chromely.CefSharp.Winapi.Browser.FrameHandlers;
    using Chromely.Core.Infrastructure;
    using Chromely.Core;

    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class FrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private static IBrowser _browser;

        ///// <summary>
        ///// Gets the browser.
        ///// </summary>
        //private static IBrowser Browser
        //{
        //    get
        //    {
        //        if (mBrowser != null)
        //        {
        //            return mBrowser;
        //        }

        //        var cefSharpFrameHandler = IoC.GetInstance<CefSharpFrameHandler>(typeof(CefSharpFrameHandler).FullName);
        //        if (cefSharpFrameHandler != null)
        //        {
        //            mBrowser = cefSharpFrameHandler.Browser;
        //        }

        //        return mBrowser;
        //    }
        //}

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see cref="IFrame"/>.
        /// </returns>
        public static IFrame GetMainFrame(this IChromelyContainer container)
        {
            return container.GetBrowser()?.MainFrame;
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="container">Chromely container</param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IFrame"/>.
        /// </returns>
        public static IFrame GetFrame(this IChromelyContainer container, string name)
        {
            return container.GetBrowser()?.GetFrame(name);
        }

        /// <summary>
        /// The get browser.
        /// </summary>
        /// <returns>
        /// The <see cref="IBrowser"/>.
        /// </returns>
        public static IBrowser GetBrowser(this IChromelyContainer container)
        {
            if (_browser != null)
            {
                return _browser;
            }

            var cefSharpFrameHandler = container.GetInstance<CefSharpFrameHandler>(typeof(CefSharpFrameHandler).FullName);
            if (cefSharpFrameHandler != null)
            {
                _browser = cefSharpFrameHandler.Browser;
            }

            return _browser;
        }
    }
}
