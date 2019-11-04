// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinapiConfigurationExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Chromely.Core.Host;
using System;

namespace Chromely.Common
{
    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class WinapiConfigurationExtension
    {
        public static ChromelyConfiguration WithHostCustomStyle(this ChromelyConfiguration configuration, WindowCreationStyle customStyle)
        {
            if (configuration != null)
            {
                configuration.HostCustomCreationStyle = customStyle;
            }

            return configuration;
        }

        public static ChromelyConfiguration WithFramelessOptions(this ChromelyConfiguration configuration, IFramelessOptions framelessOptions)
        {
            if (configuration != null)
            {
                if (configuration.HostPlacement == null)
                {
                    throw new Exception("HostPlacement cannot be null.");
                }

                configuration.HostPlacement.FramelessOptions = framelessOptions;
            }

            return configuration;
        }
    }
}
