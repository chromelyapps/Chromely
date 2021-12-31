// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

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
