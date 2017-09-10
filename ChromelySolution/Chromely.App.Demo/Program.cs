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

 namespace Chromely.App.Demo
{
    using Chromely.ChromeHosts;
    using Chromely.ChromeHosts.Winapi;
    using Chromely.Infrastructure;
    using System;
    using System.Reflection;
    using WinApi.Windows;

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();

                HostConfig config = new HostConfig();

                config.Width = 1200;
                config.Height = 900;
                config.Scheme = "http";
                config.Domain = "chromely.com";

                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                config.AppArgs = args;
                config.StartUrl = string.Format("file:///{0}Views/chromely.html", appDirectory);

                var factory = ChromeHostFactory.CreateWinapi("chromely.ico");
                using (var window = factory.CreateWindow(() => new ChromeBrowserWinapi(config),
                    "chromely", constructionParams: new FrameWindowConstructionParams()))
                {
                    // Register external url schems
                    window.RegisterExternalUrlScheme(new UrlScheme("https://github.com"));

                    // Register service assemblies
                    window.RegisterServiceAssembly(Assembly.GetExecutingAssembly());

                    // Note ensure external is valid folder.
                    string serviceAssemblyFile = @"C:\ChromelyDlls\Chromely.Service.Demo.dll";
                    window.RegisterServiceAssembly(serviceAssemblyFile);

                    // Scan assembly
                    window.ScanAssemblies();

                    window.SetSize(config.Width, config.Height);
                    window.Show();
                    return new EventLoop().Run(window);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return 0;
        }
    }
}
