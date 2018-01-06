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

namespace Chromely.Core
{
    using Chromely.Core.Infrastructure;

    public class ChromelyConfiguration
    {
        public ChromelyConfiguration()
        {
            CefLogSeverity = LogSeverity.Warning;
            CefLogFile = "logs\\chromely.cef.log";
            CefHostWidth = 1200;
            CefHostHeight = 900;
        }

        public string CefTitle { get; set; }
        public string[] CefAppArgs { get; set; }
        public string CefStartUrl { get; set; }
        public int CefHostWidth { get; set; }
        public int CefHostHeight { get; set; }
        public LogSeverity CefLogSeverity { get; set; }
        public string CefLogFile { get; set; }
        public string CefIconFile { get; set; }

        public static ChromelyConfiguration Create(IChromelyContainer container = null)
        {
            if (container != null)
            {
                IoC.Container = container;
            }

            return new ChromelyConfiguration();
        }

        public ChromelyConfiguration WithCefAppArgs(string[] args)
        {
            CefAppArgs = args;
            return this;
        }

        public ChromelyConfiguration WithCefStartUrl(string startUrl)
        {
            CefStartUrl = startUrl;
            return this;
        }

        public ChromelyConfiguration WithCefTitle(string title)
        {
            CefTitle = title;
            return this;
        }

        public ChromelyConfiguration WithCefIconFile(string iconFile)
        {
            CefIconFile = iconFile;
            return this;
        }

        public ChromelyConfiguration WithCefHostSize(int width, int height)
        {
            CefHostWidth = width;
            CefHostHeight = height;
            return this;
        }

        public ChromelyConfiguration WithCefLogSeverity(LogSeverity logSeverity)
        {
            CefLogSeverity = logSeverity;
            return this;
        }

        public ChromelyConfiguration WithCefLogFile(string cefLogFile)
        {
            CefLogFile = cefLogFile;
            return this;
        }

        public ChromelyConfiguration UseDefaultLogger(string logFile = null, bool logToConsole = true, int rollingMaxMBFileSize = 10)
        {
            var simpleLogger = new SimpleLogger(logFile, logToConsole, rollingMaxMBFileSize);
            IoC.RegisterInstance(typeof(IChromelyLogger), typeof(Log).FullName, simpleLogger);
            return this;
        }

        public ChromelyConfiguration WithLogger(IChromelyLogger logger)
        {
            Log.Logger = logger;
            return this;
        }

        public virtual ChromelyConfiguration RegisterCustomrUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, false);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        public virtual ChromelyConfiguration RegisterExternaleUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, true);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        public virtual ChromelyConfiguration RegisterSchemeHandler(string schemeName, string domainName, object handlerFactory)
        {
            return RegisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, handlerFactory));
        }

        public virtual ChromelyConfiguration RegisterSchemeHandler(ChromelySchemeHandler chromelySchemeHandler)
        {
            if (chromelySchemeHandler != null)
            {
                UrlScheme scheme = new UrlScheme(chromelySchemeHandler.SchemeName, chromelySchemeHandler.DomainName, false);
                UrlSchemeProvider.RegisterScheme(scheme);
                IoC.RegisterInstance(typeof(ChromelySchemeHandler), chromelySchemeHandler.Key, chromelySchemeHandler);
            }

            return this;
        }

        public virtual ChromelyConfiguration RegisterJsHandler(string jsMethod, object boundObject, object boundingOptions, bool registerAsync)
        {
            return RegisterJsHandler(new ChromelyJsHandler(jsMethod, boundObject, boundingOptions, registerAsync));
        }

        public virtual ChromelyConfiguration RegisterJsHandler(ChromelyJsHandler chromelyJsHandler)
        {
            if (chromelyJsHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyJsHandler), chromelyJsHandler.Key, chromelyJsHandler);
            }

            return this;
        }

        public virtual ChromelyConfiguration RegisterMessageRouterHandler(object chromelyMesssageRouterHandler)
        {
            return RegisterMessageRouterHandler(new ChromelyMesssageRouter(chromelyMesssageRouterHandler));
        }

        public virtual ChromelyConfiguration RegisterMessageRouterHandler(ChromelyMesssageRouter chromelyMesssageRouter)
        {
            if (chromelyMesssageRouter != null)
            {
                IoC.RegisterInstance(typeof(ChromelyMesssageRouter), chromelyMesssageRouter.Key, chromelyMesssageRouter);
            }

            return this;
        }
    }
}
