// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerAssemblyInfo.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace Chromely.Core.Network
{
    /// <summary>
    /// The controller assembly info.
    /// </summary>
    public class ControllerAssemblyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerAssemblyInfo"/> class.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public ControllerAssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
            Key = assembly.FullName;
        }

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is scanned.
        /// </summary>
        public bool IsScanned { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get;  }
    }
}
