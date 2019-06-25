// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyHostApi.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace Chromely.Core
{
    /// <summary>
    /// List of supported host interface apis.
    /// Attention! Not every api is supported on all platforms.
    /// </summary>
    public enum ChromelyHostApi
    {
        None,

        Winapi,
        Gtk,
        Libui
    }
}
