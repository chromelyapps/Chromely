// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerPropertyAttribute.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The controller property attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        public string Route { get; set; }
    }
}