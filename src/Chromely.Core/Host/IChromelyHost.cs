// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyHost.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Host
{
    /// <summary>
    /// The ChromelyHost interface.
    /// </summary>
    public interface IChromelyHost
    {
        /// <summary>
        /// Gets the host config.
        /// </summary>
        ChromelyConfiguration HostConfig { get; }

        /// <summary>
        /// The register scheme handlers.
        /// </summary>
        void RegisterSchemeHandlers();

        /// <summary>
        /// The register message routers.
        /// </summary>
        void RegisterMessageRouters();
    }
}