// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyServiceProvider.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace Chromely.Core.Network
{
    public interface IChromelyServiceProvider
    {
        void RegisterServiceAssembly(string filename);
        void RegisterServiceAssembly(Assembly assembly);
        void ScanAssemblies();
        void RegisterRoutes();
    }
}
