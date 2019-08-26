// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlSchemeProvider.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Chromely.Core.Infrastructure
{
    /// <summary>
    /// The url scheme provider.
    /// </summary>
    public static class UrlSchemeProvider
    {
        /// <summary>
        /// The lock object.
        /// </summary>
        private static readonly object ObjLock = new object();

        /// <summary>
        /// The url schemes.
        /// </summary>
        private static readonly List<UrlScheme> UrlSchemes = new List<UrlScheme>();

        /// <summary>
        /// Registers scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        public static void RegisterScheme(UrlScheme scheme)
        {
            lock (ObjLock)
            {
                if (!UrlSchemes.Contains(scheme))
                {
                    UrlSchemes.Add(scheme);
                }
            }
        }

        /// <summary>
        /// Checks if url registered is external.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsUrlRegisteredExternal(string url)
        {
            lock (ObjLock)
            {
                return UrlSchemes.Any(x => (x.IsUrlOfSameScheme(url) && x.UrlSchemeType == UrlSchemeType.External));
            }
        }

        /// <summary>
        /// Checks if url registered is a command url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsUrlRegisteredCommand(string url)
        {
            lock (ObjLock)
            {
                return UrlSchemes.Any(x => (x.IsUrlOfSameScheme(url) && x.UrlSchemeType == UrlSchemeType.Command));
            }
        }

        /// <summary>
        /// Checks if url registered is for custom scheme.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsUrlOfRegisteredCustomScheme(string url)
        {
            lock (ObjLock)
            {
                return UrlSchemes.Any(x => (x.IsUrlOfSameScheme(url) && x.UrlSchemeType == UrlSchemeType.Custom));
            }
        }
    }
}
