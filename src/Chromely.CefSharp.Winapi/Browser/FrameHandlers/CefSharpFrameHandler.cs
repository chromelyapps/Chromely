// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSharpFrameHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using global::CefSharp;

namespace Chromely.CefSharp.Winapi.Browser.FrameHandlers
{
    /// <summary>
    /// The CefSharp frame handler.
    /// </summary>
    internal class CefSharpFrameHandler
    {
        /// <summary>
        /// The browser.
        /// </summary>
        private readonly IBrowser _browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpFrameHandler"/> class.
        /// </summary>
        /// <param name="browser">
        /// The browser.
        /// </param>
        public CefSharpFrameHandler(IBrowser browser)
        {
            _browser = browser;
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public IBrowser Browser
        {
            get
            {
                if (_browser == null)
                {
                    throw new Exception("Browser object cannot be null.");
                }

                return _browser;
            }
        }

        /// <summary>
        /// Gets the get frame identifiers.
        /// </summary>
        public List<long> GetFrameIdentifiers => Browser.GetFrameIdentifiers()?.ToList();

        /// <summary>
        /// Gets the get frame names.
        /// </summary>
        public List<string> GetFrameNames => Browser.GetFrameNames();

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
            return Browser.MainFrame;
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
            return Browser.GetFrame(frameName);
        }
    }
}
