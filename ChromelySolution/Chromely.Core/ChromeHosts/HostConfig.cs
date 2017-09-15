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

namespace Chromely.Core.ChromeHosts
{
    using Chromely.Core.CefGlueBrowser;
    using Chromely.Core.Infrastructure;
   
    public class HostConfig
    {
        public HostConfig()
        {
            LogSeverity = LogSeverity.Warning;
            LogFile = "Chromely.Cef.Log";
        }

        public string[] AppArgs { get; set; }
        public string StartUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public LogSeverity LogSeverity { get; set; }
        public string LogFile { get; set; }

        public virtual void RgisterDefaultSchemeHandler(string schemeName, string domainName)
        {
            RgisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, new DefaultSchemeHandlerFactory()));
        }

        public virtual void RgisterSchemeHandler(string schemeName, string domainName, object handlerFactory)
        {
            RgisterSchemeHandler(new ChromelySchemeHandler(schemeName, domainName, handlerFactory));
        }

        public virtual void RgisterSchemeHandler(ChromelySchemeHandler chromelySchemeHandler)
        {
            if (chromelySchemeHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelySchemeHandler), chromelySchemeHandler.Key, chromelySchemeHandler);
            }
        }

        public virtual void RgisterJsHandler(string jsMethod, object boundObject, bool registerAsync)
        {
            RgisterJsHandler(new ChromelyJsHandler(jsMethod, boundObject, registerAsync));
        }

        public virtual void RgisterJsHandler(ChromelyJsHandler chromelyJsHandler)
        {
            if (chromelyJsHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyJsHandler), chromelyJsHandler.Key, chromelyJsHandler);
            }
        }
    }
}
