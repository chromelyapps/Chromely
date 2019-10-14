// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyServiceProvider.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The ChromelyServiceProvider interface.
    /// </summary>
    public interface IChromelyServiceProvider
    {
        /// <summary>
        /// Registers url scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        void RegisterUrlScheme(UrlScheme scheme);

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        void RegisterServiceAssembly(string filename);

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        void RegisterServiceAssembly(Assembly assembly);

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        void RegisterServiceAssemblies(string folder);

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="fileNames">
        /// The filenames.
        /// </param>
        void RegisterServiceAssemblies(List<string> fileNames);

        /// <summary>
        /// Scan assemblies.
        /// </summary>
        void ScanAssemblies();
    }
}
