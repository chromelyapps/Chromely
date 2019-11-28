// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterServiceAssembliesExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Chromely.Core.Infrastructure;

namespace Chromely.Core.Network
{
    /// <summary>
    /// The registration of service assemblies extension.
    /// </summary>
    public static class RegisterServiceAssembliesExtension
    {
        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="serviceAssemblies">
        /// The service assemblies.
        /// </param>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public static void RegisterServiceAssembly(this List<ControllerAssemblyInfo> serviceAssemblies, string filename)
        {
            if (!File.Exists(filename))
            {
                Logger.Instance.Log.Error($"Assembly file: {filename} does not exist.");
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(filename);
                serviceAssemblies.RegisterServiceAssembly(assembly);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="serviceAssemblies">
        /// The service assemblies.
        /// </param>
        /// <param name="filenames">
        /// The filenames.
        /// </param>
        public static void RegisterServiceAssemblies(this List<ControllerAssemblyInfo> serviceAssemblies, List<string> filenames)
        {
            if (filenames != null && filenames.Any())
            {
                foreach (var filename in filenames)
                {
                    serviceAssemblies.RegisterServiceAssembly(filename);
                }
            }
        }

        /// <summary>
        /// Registers service assemblies.
        /// </summary>
        /// <param name="serviceAssemblies">
        /// The service assemblies.
        /// </param>
        /// <param name="folderPath">
        /// The folder path.
        /// </param>
        public static void RegisterServiceAssemblies(this List<ControllerAssemblyInfo> serviceAssemblies, string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (var filename in Directory.EnumerateFiles(folderPath, "*.dll"))
                {
                    serviceAssemblies.RegisterServiceAssembly(filename);
                }
            }
        }

        /// <summary>
        /// Registers service assembly.
        /// </summary>
        /// <param name="serviceAssemblies">
        /// The service assemblies.
        /// </param>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        public static void RegisterServiceAssembly(this List<ControllerAssemblyInfo> serviceAssemblies, Assembly assembly)
        {
            if (serviceAssemblies == null)
            {
                serviceAssemblies = new List<ControllerAssemblyInfo>();
            }

            if (assembly != null)
            {
                if (!serviceAssemblies.Select(x => x.Key).Contains(assembly.FullName))
                {
                    serviceAssemblies.Add(new ControllerAssemblyInfo(assembly));
                }
            }
        }
    }
}
