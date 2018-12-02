// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpFrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Browser.FrameHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::CefSharp;

    /// <summary>
    /// The CefSharp frame handler.
    /// </summary>
    internal class CefSharpFrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private readonly IBrowser browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpFrameHandler"/> class.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public CefSharpFrameHandler(IBrowser browser)
        {
            this.browser = browser;
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public IBrowser Browser
        {
            get
            {
                if (this.browser == null)
                {
                    throw new Exception("Browser object cannot be null.");
                }

                return this.browser;
            }
        }

        /// <summary>
        /// Gets the get frame identifiers.
        /// </summary>
        public List<long> GetFrameIdentifiers => this.Browser.GetFrameIdentifiers()?.ToList();

        /// <summary>
        /// Gets the get frame names.
        /// </summary>
        public List<string> GetFrameNames => this.Browser.GetFrameNames();

        /// <summary>
        /// The get main frame.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IFrame</cref>
        ///     </see>
        ///     .
        /// </returns>
        public IFrame GetMainFrame()
        {
            return this.Browser.MainFrame;
        }

        /// <summary>
        /// The get frame.
        /// </summary>
        /// <param name="frameName">
        /// The frame name.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IFrame</cref>
        ///     </see>
        ///     .
        /// </returns>
        public IFrame GetFrame(string frameName)
        {
            return this.Browser.GetFrame(frameName);
        }
    }
}
