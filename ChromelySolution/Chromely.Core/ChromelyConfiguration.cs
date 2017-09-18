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
    using Chromely.Core.CefGlueBrowser;
    using Chromely.Core.Infrastructure;

    public class ChromelyConfiguration
    {
        public ChromelyConfiguration()
        {
            LogSeverity = LogSeverity.Warning;
            CefLogFile = "logs\\chromely.cef.log";
            LogFile = "logs\\chromely.log";
        }

        public string[] AppArgs { get; set; }
        public string StartUrl { get; set; }
        public int HostWidth { get; set; }
        public int HostHeight { get; set; }
        public LogSeverity LogSeverity { get; set; }
        public string CefLogFile { get; set; }
        public string LogFile { get; set; }

        public static ChromelyConfiguration Create()
        {
            return new ChromelyConfiguration();
        }

        public ChromelyConfiguration WithAppArgs(string[] args)
        {
            AppArgs = args;
            return this;
        }

        public ChromelyConfiguration WithStartUrl(string startUrl)
        {
            StartUrl = startUrl;
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

        public ChromelyConfiguration WithCefLogFile(string cefLogFile)
        {
            CefLogFile = cefLogFile;
            return this;
        }

        public ChromelyConfiguration WithLogFile(string logFile)
        {
            LogFile = logFile;
            Log.LoggerFile = logFile;
            return this;
        }

        public virtual ChromelyConfiguration RgisterCustomrUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, false);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        public virtual ChromelyConfiguration RgisterExternaleUrlScheme(string schemeName, string domainName)
        {
            UrlScheme scheme = new UrlScheme(schemeName, domainName, true);
            UrlSchemeProvider.RegisterScheme(scheme);
            return this;
        }

        public virtual ChromelyConfiguration RgisterSchemeHandler(string schemeName, string domainName, object handlerFactory)
        {
            RgisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, handlerFactory));
            return this;
        }

        public virtual ChromelyConfiguration RgisterSchemeHandler(ChromelySchemeHandler chromelySchemeHandler)
        {
            if (chromelySchemeHandler != null)
            {
                UrlScheme scheme = new UrlScheme(chromelySchemeHandler.SchemeName, chromelySchemeHandler.DomainName, false);
                UrlSchemeProvider.RegisterScheme(scheme);
                IoC.RegisterInstance(typeof(ChromelySchemeHandler), chromelySchemeHandler.Key, chromelySchemeHandler);
            }

            return this;
        }

        public virtual ChromelyConfiguration RgisterJsHandler(string jsMethod, object boundObject, object boundingOptions, bool registerAsync)
        {
            RgisterJsHandler(new ChromelyJsHandler(jsMethod, boundObject, boundingOptions, registerAsync));
            return this;
        }

        public virtual ChromelyConfiguration RgisterJsHandler(ChromelyJsHandler chromelyJsHandler)
        {
            if (chromelyJsHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyJsHandler), chromelyJsHandler.Key, chromelyJsHandler);
            }

            return this;
        }
    }
}
