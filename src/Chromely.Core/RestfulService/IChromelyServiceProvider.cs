// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyServiceProvider.cs" company="Chromely">
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
    using System.Collections.Generic;
    using System.Reflection;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The ChromelyServiceProvider interface.
    /// </summary>
    public interface IChromelyServiceProvider
    {
        /// <summary>
        /// Gets the service assemblies.
        /// </summary>
        List<Assembly> ServiceAssemblies { get; }

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
        /// <param name="filenames">
        /// The filenames.
        /// </param>
        void RegisterServiceAssemblies(List<string> filenames);

        /// <summary>
        /// Scan assemblies.
        /// </summary>
        void ScanAssemblies();
    }
}
