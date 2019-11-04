// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using global::CefSharp;
using Chromely.CefSharp.Winapi.Browser.FrameHandlers;
using Chromely.Core.Infrastructure;

namespace Chromely.CefSharp.Winapi
{
    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class FrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private static IBrowser mBrowser;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        private static IBrowser Browser
        {
            get
            {
                if (mBrowser != null)
                {
                    return mBrowser;
                }

                var cefSharpFrameHandler = IoC.GetInstance<CefSharpFrameHandler>(typeof(CefSharpFrameHandler).FullName);
                if (cefSharpFrameHandler != null)
                {
                    mBrowser = cefSharpFrameHandler.Browser;
                }

                return mBrowser;
            }
        }

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see cref="IFrame"/>.
        /// </returns>
        public static IFrame GetMainFrame()
        {
            return Browser?.MainFrame;
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="IFrame"/>.
        /// </returns>
        public static IFrame GetFrame(string name)
        {
            return Browser?.GetFrame(name);
        }

        /// <summary>
        /// The get browser.
        /// </summary>
        /// <returns>
        /// The <see cref="IBrowser"/>.
        /// </returns>
        public static IBrowser GetBrowser()
        {
            return Browser;
        }
    }
}
