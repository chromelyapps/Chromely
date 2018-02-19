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

namespace Chromely.Core.Infrastructure
{
    using System;

    public class ChromelySchemeHandler
    {
        public ChromelySchemeHandler()
        {
            Key = Guid.NewGuid().ToString();
        }

        public ChromelySchemeHandler(string schemeName, string domainName, object handlerFactory, bool isSecured = false, bool isCorsEnabled = true)
        {
            Key = Guid.NewGuid().ToString();
            SchemeName = schemeName;
            DomainName = domainName;
            HandlerFactory = handlerFactory;
            IsSecure = isSecured;
            IsCorsEnabled = isCorsEnabled;
        }

        public string Key { get; private set; }
        public string SchemeName { get; set; }
        public string DomainName { get; set; }
        /// <summary>
        //     If true the scheme will be treated with the same security rules as those applied
        //     to "https" URLs. For example, loading this scheme from other secure schemes will
        //     not trigger mixed content warnings.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        //     If true the scheme can be sent CORS requests. This value should be true in most
        //     cases where IsStandard is true.
        /// </summary>
        public bool IsCorsEnabled { get; set; }
        public object HandlerFactory { get; set; }
    }
}
