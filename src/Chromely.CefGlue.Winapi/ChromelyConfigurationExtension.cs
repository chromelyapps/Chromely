// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core;

namespace Chromely.CefGlue.Winapi
{
    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class ChromelyConfigurationExtension
    {
        public static ChromelyConfiguration WithHostCustomStyle(this ChromelyConfiguration configuration, WindowCreationStyle customStyle)
        {
            if (configuration != null)
            {
                configuration.HostCustomCreationStyle = customStyle;
            }

            return configuration;
        }
    }
}
