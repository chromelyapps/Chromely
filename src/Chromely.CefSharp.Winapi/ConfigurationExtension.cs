// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Chromely.Core.Infrastructure;

namespace Chromely.CefSharp.Winapi
{
    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Use default Javascript object handler.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <summary>
        /// Sets use default Javascript object handler flag.
        /// </summary>
        /// <param name="objectNameToBind">
        /// The object name to bind.
        /// </param>
        /// <param name="registerAsync">
        /// The register async.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public static ChromelyConfiguration UseDefaultJsHandler(this ChromelyConfiguration configuration, string objectNameToBind, bool registerAsync)
        {
            return configuration.RegisterJsHandler(new ChromelyJsHandler(objectNameToBind, registerAsync));
        }

        /// <summary>
        /// Registers Javascript object handler.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="javascriptMethod">
        /// The javascript method.
        /// </param>
        /// <param name="boundObject">
        /// The bound object.
        /// </param>
        /// <param name="boundingOptions">
        /// The bounding options.
        /// </param>
        /// <param name="registerAsync">
        /// The register async.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public static ChromelyConfiguration RegisterJsHandler(this ChromelyConfiguration configuration, string javascriptMethod, object boundObject, object boundingOptions, bool registerAsync)
        {
            return configuration.RegisterJsHandler(new ChromelyJsHandler(javascriptMethod, boundObject, boundingOptions, registerAsync));
        }

        /// <summary>
        /// Registers Javascript object handler.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="chromelyJsHandler">
        /// The chromely js handler.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public static ChromelyConfiguration RegisterJsHandler(this ChromelyConfiguration configuration, ChromelyJsHandler chromelyJsHandler)
        {
            if (chromelyJsHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyJsHandler), chromelyJsHandler.Key, chromelyJsHandler);
            }

            return configuration;
        }
    }
}
