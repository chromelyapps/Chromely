// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginCrashedEventArgs.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.CefGlue.Browser.EventParams
{
    /// <summary>
    /// The plugin crashed event args.
    /// </summary>
    public class PluginCrashedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginCrashedEventArgs"/> class.
        /// </summary>
        /// <param name="pluginPath">
        /// The plugin path.
        /// </param>
        public PluginCrashedEventArgs(string pluginPath)
        {
            PluginPath = pluginPath;
        }

        /// <summary>
        /// Gets the plugin path.
        /// </summary>
        public string PluginPath { get; }
    }
}
