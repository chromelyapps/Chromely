/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.RestfulService
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class RegisterServiceAssembliesExtension
    {
        public static void RegisterServiceAssembly(this List<Assembly> serviceAssemblies, string filename)
        {
            if (File.Exists(filename))
            {
                serviceAssemblies.RegisterServiceAssembly(Assembly.LoadFile(filename));
            }
        }

        public static void RegisterServiceAssemblies(this List<Assembly> serviceAssemblies, List<string> filenames)
        {
            if (filenames != null && (filenames.Count > 0))
            {
                foreach (var filename in filenames)
                {
                    serviceAssemblies.RegisterServiceAssembly(filename);
                }
            }
        }

        public static void RegisterServiceAssemblies(this List<Assembly> serviceAssemblies, string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (string filename in Directory.EnumerateFiles(folderPath, "*.dll"))
                {
                    serviceAssemblies.RegisterServiceAssembly(filename);
                }
            }
        }

        public static void RegisterServiceAssembly(this List<Assembly> serviceAssemblies, Assembly assembly)
        {
            if (serviceAssemblies == null)
            {
                serviceAssemblies = new List<Assembly>();
            }

            if (assembly != null)
            {
                serviceAssemblies.Add(assembly);
            }
        }
    }
}
