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

namespace Chromely.CefGlue.Gtk.Win.Demo
{
    using Chromely.CefGlue.Gtk.Browser.Handlers;
    using Chromely.CefGlue.Gtk.ChromeHost;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using System;
    using System.Reflection;

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                HostHelpers.SetupDefaultExceptionHandlers();

                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string startUrl = string.Format("file:///{0}Views/chromely.html", appDirectory);

                ChromelyConfiguration config = ChromelyConfiguration
                                              .Create()
                                              .WithCefTitle("chromely")
                                              .WithCefIconFile("chromely.ico")
                                              .WithCefAppArgs(args)
                                              .WithCefHostSize(1200, 900)
                                              .WithCefLogFile("logs\\chromely.cef_new.log")
                                              .WithCefStartUrl(startUrl)
                                              .WithCefLogSeverity(LogSeverity.Info)
                                              .UseDefaultLogger("logs\\chromely_new.log", true)
                                              .RegisterSchemeHandler("http", "chromely.com", new CefGlueSchemeHandlerFactory());

                using (var app = new CefGlueBrowserHost(config))
                {
                    // Register external url schems
                    app.RegisterExternalUrlScheme(new UrlScheme("https://github.com/mattkol/Chromely", true));

                    // Register service assemblies
                    app.RegisterServiceAssembly(Assembly.GetExecutingAssembly());

                    // Note ensure external is valid folder.
                    // Uncomment to register external restful service dlls
                    // string serviceAssemblyFile = @"C:\ChromelyDlls\Chromely.Service.Demo.dll";
                    // app.RegisterServiceAssembly(serviceAssemblyFile);

                    // Scan assemblies for Controller routes 
                    app.ScanAssemblies();

                    return app.Run(args);
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

