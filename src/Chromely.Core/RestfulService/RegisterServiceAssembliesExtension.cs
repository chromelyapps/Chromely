// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterServiceAssembliesExtension.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.RestfulService
{
    using Chromely.Core.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

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
                Log.Error($"Assembly file: {filename} does not exist.");
            }

            try
            {
                var assembly = Assembly.LoadFrom(filename);
                serviceAssemblies.RegisterServiceAssembly(assembly);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
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
            if (filenames != null && (filenames.Count > 0))
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
