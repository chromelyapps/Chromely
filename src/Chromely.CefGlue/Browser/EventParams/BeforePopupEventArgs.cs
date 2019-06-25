// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforePopupEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.EventParams
{
    /// <summary>
    /// The before popup event args.
    /// </summary>
    public class BeforePopupEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforePopupEventArgs"/> class.
        /// </summary>
        /// <param name="frame">
        /// The frame.
        /// </param>
        /// <param name="targetUrl">
        /// The target url.
        /// </param>
        /// <param name="targetFrameName">
        /// The target frame name.
        /// </param>
        /// <param name="popupFeatures">
        /// The popup features.
        /// </param>
        /// <param name="windowInfo">
        /// The window info.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="noJavascriptAccess">
        /// The no javascript access.
        /// </param>
        public BeforePopupEventArgs(
            CefFrame frame,
            string targetUrl,
            string targetFrameName,
            CefPopupFeatures popupFeatures,
            CefWindowInfo windowInfo,
            CefClient client,
            CefBrowserSettings settings,
            bool noJavascriptAccess)
        {
                this.Frame = frame;
                this.TargetUrl = targetUrl;
                this.TargetFrameName = targetFrameName;
                this.PopupFeatures = popupFeatures;
                this.WindowInfo = windowInfo;
                this.Client = client;
                this.Settings = settings;
                this.NoJavascriptAccess = noJavascriptAccess;
        }

        /// <summary>
        /// Gets or sets a value indicating whether no javascript access.
        /// </summary>
        public bool NoJavascriptAccess { get; set; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public CefBrowserSettings Settings { get; private set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        public CefClient Client { get; set; }

        /// <summary>
        /// Gets the window info.
        /// </summary>
        public CefWindowInfo WindowInfo { get; private set; }

        /// <summary>
        /// Gets the popup features.
        /// </summary>
        public CefPopupFeatures PopupFeatures { get; private set; }

        /// <summary>
        /// Gets the target frame name.
        /// </summary>
        public string TargetFrameName { get; private set; }

        /// <summary>
        /// Gets the target url.
        /// </summary>
        public string TargetUrl { get; private set; }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        public CefFrame Frame { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
