// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelySchemeHandler.cs" company="Chromely">
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

namespace Chromely.Core
{
    using System;

    /// <summary>
    /// The chromely scheme handler.
    /// </summary>
    public class ChromelySchemeHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelySchemeHandler"/> class.
        /// </summary>
        public ChromelySchemeHandler()
        {
            this.Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelySchemeHandler"/> class.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name. Sample url can be "https://google.com". Scheme name os "https".
        /// </param>
        /// <param name="domainName">
        /// The domain name. Sample url can be "https://google.com". Domain name os "google.com".
        /// </param>
        /// <param name="useDefaultResource">
        /// The use default resource handler flag.
        /// </param>
        /// <param name="useDefaultHttp">
        /// The use default http handler flag.
        /// </param>
        /// <param name="isSecured">
        /// The is domain set secured flag.
        /// </param>
        /// <param name="isCorsEnabled">
        /// The is CORS enabled for scheme handling flag..
        /// </param>
        public ChromelySchemeHandler(string schemeName, string domainName, bool useDefaultResource, bool useDefaultHttp, bool isSecured = false, bool isCorsEnabled = true)
        {
            this.Key = $"{schemeName}_{domainName}";
            this.SchemeName = schemeName;
            this.DomainName = domainName;
            this.HandlerFactory = null;
            this.UseDefaultResource = useDefaultResource;
            this.UseDefaultHttp = useDefaultHttp;
            this.IsSecure = isSecured;
            this.IsCorsEnabled = isCorsEnabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelySchemeHandler"/> class.
        /// </summary>
        /// <param name="schemeName">
        /// The scheme name. Sample url can be "https://google.com". Scheme name os "https".
        /// </param>
        /// <param name="domainName">
        /// The domain name. Sample url can be "https://google.com". Domain name os "google.com".
        /// </param>
        /// <param name="handlerFactory">
        /// The handler factory.
        /// </param>
        /// <param name="isSecured">
        /// The is domain set secured flag.
        /// </param>
        /// <param name="isCorsEnabled">
        /// The is CORS enabled for scheme handling flag..
        /// </param>
        public ChromelySchemeHandler(string schemeName, string domainName, object handlerFactory, bool isSecured = false, bool isCorsEnabled = true)
        {
            this.Key = $"{schemeName}_{domainName}";
            this.SchemeName = schemeName;
            this.DomainName = domainName;
            this.HandlerFactory = handlerFactory;
            this.UseDefaultResource = false;
            this.UseDefaultHttp = false;
            this.IsSecure = isSecured;
            this.IsCorsEnabled = isCorsEnabled;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the scheme name. Sample url can be "https://google.com". Scheme name os "https".
        /// </summary>
        public string SchemeName { get; set; }

        /// <summary>
        /// Gets or sets the domain name. Sample url can be "https://google.com". Domain name os "google.com".
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is secure.
        /// If true the scheme will be treated with the same security rules as those applied
        /// to "https" URLs. For example, loading this scheme from other secure schemes will
        /// not trigger mixed content warnings.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is cors enabled.
        /// If true the scheme can be sent CORS requests. This value should be true in most
        /// cases where IsStandard is true.
        /// </summary>
        public bool IsCorsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use default http.
        /// </summary>
        public bool UseDefaultHttp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use default resource.
        /// </summary>
        public bool UseDefaultResource { get; set; }

        /// <summary>
        /// Gets or sets the handler factory.
        /// </summary>
        public object HandlerFactory { get; set; }
    }
}
