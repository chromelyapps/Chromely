// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelySchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core
{
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
            Key = Guid.NewGuid().ToString();
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
            Key = $"{schemeName}_{domainName}";
            SchemeName = schemeName;
            DomainName = domainName;
            HandlerFactory = null;
            UseDefaultResource = useDefaultResource;
            UseDefaultHttp = useDefaultHttp;
            IsSecure = isSecured;
            IsCorsEnabled = isCorsEnabled;
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
            Key = $"{schemeName}_{domainName}";
            SchemeName = schemeName;
            DomainName = domainName;
            HandlerFactory = handlerFactory;
            UseDefaultResource = false;
            UseDefaultHttp = false;
            IsSecure = isSecured;
            IsCorsEnabled = isCorsEnabled;
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
