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
    using System.Collections.Generic;

    public class ChromelyConfiguration
    {
        public ChromelyConfiguration()
        {
            PerformDependencyCheck = false;
            LogSeverity = LogSeverity.Warning;
            LogFile = "logs\\chromely.cef.log";
            HostWidth = 1200;
            HostHeight = 900;
            Locale = "en-US";
            CommandLineArgs = new Dictionary<string, string>();
            CustomSettings = new Dictionary<string, object>();
        }

        public string HostTitle { get; set; }
        public int HostWidth { get; set; }
        public int HostHeight { get; set; }
        public string HostIconFile { get; set; }
        public bool PerformDependencyCheck { get; set; }
        public string[] AppArgs { get; set; }
        public string StartUrl { get; set; }
        public LogSeverity LogSeverity { get; set; }
        public string LogFile { get; set; }
        public string Locale { get; set; }
        public Dictionary<string, string> CommandLineArgs { get; set; }
        public Dictionary<string, object> CustomSettings { get; set; }

        public static ChromelyConfiguration Create(IChromelyContainer container = null)
        {
            if (container != null)
            {
                IoC.Container = container;
            }

            return new ChromelyConfiguration();
        }

        public ChromelyConfiguration WithAppArgs(string[] args)
        {
            AppArgs = args;
            return this;
        }

        public ChromelyConfiguration WithDependencyCheck(bool checkDependencies)
        {
            PerformDependencyCheck = checkDependencies;
            return this;
        }
        
        public ChromelyConfiguration WithStartUrl(string startUrl)
        {
            StartUrl = startUrl;
            return this;
        }

        public ChromelyConfiguration WithHostTitle(string hostTitle)
        {
            HostTitle = hostTitle;
            return this;
        }

        public ChromelyConfiguration WithHostIconFile(string hostIconFile)
        {
            HostIconFile = hostIconFile;
            return this;
        }

        public ChromelyConfiguration WithHostSize(int width, int height)
        {
            HostWidth = width;
            HostHeight = height;
            return this;
        }

        public ChromelyConfiguration WithLogSeverity(LogSeverity logSeverity)
        {
            LogSeverity = logSeverity;
            return this;
        }

        public ChromelyConfiguration WithLogFile(string logFile)
        {
            LogFile = logFile;
            return this;
        }

        public ChromelyConfiguration WithLocale(string locale)
        {
            Locale = locale;
            return this;
        }

        public ChromelyConfiguration UseDefaultLogger(string logFile = null, bool logToConsole = true, int rollingMaxMBFileSize = 10)
        {
            var simpleLogger = new SimpleLogger(logFile, logToConsole, rollingMaxMBFileSize);
            IoC.RegisterInstance(typeof(IChromelyLogger), typeof(Log).FullName, simpleLogger);
            return this;
        }

        public ChromelyConfiguration UseDefaultResourceSchemeHandler(string schemeName, string domainName)
        {
            var handler = new ChromelySchemeHandler(schemeName, domainName, true, false);
            RegisterSchemeHandler(handler);

            return this;
        }

        public ChromelyConfiguration UseDefaultHttpSchemeHandler(string schemeName, string domainName)
        {
            var handler = new ChromelySchemeHandler(schemeName, domainName, false, true);
            RegisterSchemeHandler(handler);

            return this;
        }

        public ChromelyConfiguration WithLogger(IChromelyLogger logger)
        {
            Log.Logger = logger;
            return this;
        }

        public ChromelyConfiguration WithCommandLineArg(string name, string value)
        {
            if (CommandLineArgs == null)
            {
                CommandLineArgs = new Dictionary<string, string>();
            }

            CommandLineArgs[name] = value;
            return this;
        }

        public ChromelyConfiguration WithCustomSetting(string propertyName, object value)
        {
            if (CustomSettings == null)
            {
                CustomSettings = new Dictionary<string, object>();
            }

            CustomSettings[propertyName] = value;
            return this;
        }

        public virtual ChromelyConfiguration RegisterCustomrUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, false);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        public virtual ChromelyConfiguration RegisterExternalUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, true);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        public virtual ChromelyConfiguration RegisterSchemeHandler(string schemeName, string domainName, object handler)
        {
            return RegisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, handler));
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
