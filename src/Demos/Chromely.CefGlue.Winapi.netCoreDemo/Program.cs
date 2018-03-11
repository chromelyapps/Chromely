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

namespace Chromely.CefGlue.Winapi.netCoreDemo
{
    using System;
    using System.Reflection;
    using Chromely.CefGlue.Winapi.Browser.Handlers;
    using Chromely.CefGlue.Winapi.ChromeHost;
    using Chromely.Core;
    using Chromely.Core.Helpers;
    using Chromely.Core.Infrastructure;
    using WinApi.Windows;

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();

                /*
                * Start url (load html) options:
                */

                // Options 1 - real standard urls 
                // string startUrl = "https://google.com";

                // Options 2 - using local resource file handling with default local scheme handler 
                // Requires - (sample) UseDefaultResourceSchemeHandler("local", string.Empty)
                // string startUrl = "local://app/chromely.html";

                // Options 3 - using file protocol - using default scheme handler for Ajax/Http requests
                // Requires - (sample) UseDefaultResourceSchemeHandler("local", string.Empty)
                // Requires - (sample) UseDefaultHttpSchemeHandler("http", "chromely.com")
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string startUrl = string.Format("file:///{0}app/chromely_with_ajax.html", appDirectory);

                ChromelyConfiguration config = ChromelyConfiguration
                                              .Create()
                                              .WithAppArgs(args)
                                              .WithHostSize(1200, 900)
                                              .WithLogFile("logs\\chromely.cef_new.log")
                                              .WithStartUrl(startUrl)
                                              .WithLogSeverity(LogSeverity.Info)
                                              .UseDefaultLogger("logs\\chromely_new.log", true)
                                              .UseDefaultResourceSchemeHandler("local", string.Empty)
                                              .UseDefaultHttpSchemeHandler("http", "chromely.com")

                                              // The single process should only be used for debugging purpose.
                                              // For production, this should not be needed when the app is published and an cefglue_winapi_netcoredemo.exe 
                                              // is created.

                                              // Alternate approach for multi-process, is to add a subprocess application
                                              //.WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, full_path_to_subprocess)
                                              .WithCustomSetting(CefSettingKeys.SingleProcess, true);
                                              

                var factory = WinapiHostFactory.Init("chromely.ico");
                using (var window = factory.CreateWindow(() => new CefGlueBrowserHost(config),
                    "chromely", constructionParams: new FrameWindowConstructionParams()))
                {
                    // Register external url schems
                    window.RegisterUrlScheme(new UrlScheme("https://github.com/mattkol/Chromely", true));
                    //window.RegisterUrlScheme(new UrlScheme("https://google.com", true));

                    /*
                     * Register service assemblies
                     * Uncomment relevant part to register assemblies
                     */

                    // 1. Register current/local assembly:
                    window.RegisterServiceAssembly(Assembly.GetExecutingAssembly());

                    // 2. Register external assembly with file name:
                    //string serviceAssemblyFile = @"C:\ChromelyDlls\Chromely.Service.Demo.dll";
                    //string serviceAssemblyFile = @"Chromely.Service.Demo.dll";
                    //window.RegisterServiceAssembly(serviceAssemblyFile);

                    // 3. Register external assemblies with list of filenames:
                    //string serviceAssemblyFile1 = @"C:\ChromelyDlls\Chromely.Service.Demo.dll";
                    //List<string> filenames = new List<string>();
                    //filenames.Add(serviceAssemblyFile1);
                    //window.RegisterServiceAssemblies(filenames);

                    // 4. Register external assemblies directory:
                    string serviceAssembliesFolder = @"C:\ChromelyDlls";
                    window.RegisterServiceAssemblies(serviceAssembliesFolder);

                    // Scan assemblies for Controller routes 
                    window.ScanAssemblies();

                    window.SetSize(config.HostWidth, config.HostHeight);
                    window.CenterToScreen();
                    window.Show();
                    return new HostEventLoop().Run(window);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return 0;
        }
    }
}
