// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionNameMapper.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Chromely.Core.RestfulService;

namespace Chromely.CefGlue.Browser.ServerHandlers
{
    /// <summary>
    /// The socket connection name mapper.
    /// </summary>
    public static class ConnectionNameMapper
    {
        /// <summary>
        /// The connection name.
        /// </summary>
        private const string ConnectionName = "name";

        /// <summary>
        /// The lock object.
        /// </summary>
        private static readonly object ObjLock = new object();

        /// <summary>
        /// The conn name dict.
        /// </summary>
        private static readonly Dictionary<string, int> ConnNameDict = new Dictionary<string, int>();

        /// <summary>
        /// Gets the connection ids.
        /// </summary>
        public static List<int> ConnectionIds
        {
            get
            {
                lock (ObjLock)
                {
                    return ConnNameDict.Values.ToList();
                }
            }
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="url">
        /// The name.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        public static void Add(string url, int connectionId)
        {
            lock (ObjLock)
            {
                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                var nameValuePairs = url.GetParameters();
                if (nameValuePairs == null)
                {
                    return;
                }

                if (!nameValuePairs.Keys.Contains(ConnectionName))
                {
                    return;
                }

                var nameValuePair = nameValuePairs.FirstOrDefault(
                    x => x.Key.Equals(ConnectionName, StringComparison.InvariantCultureIgnoreCase));

                if (string.IsNullOrEmpty(nameValuePair.Value))
                {
                    return;
                }

                if (!ConnNameDict.ContainsKey(nameValuePair.Value.ToLower()))
                {
                    ConnNameDict.Add(nameValuePair.Value.ToLower(), connectionId);
                }
            }
        }

        /// <summary>
        /// The get name.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetName(int connectionId)
        {
            lock (ObjLock)
            {
                if (ConnNameDict.ContainsValue(connectionId))
                {
                    var mapper = ConnNameDict.FirstOrDefault(x => x.Value == connectionId);
                    return mapper.Key;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// The get connection id.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetConnectionId(string name)
        {
            lock (ObjLock)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return 0;
                }

                if (ConnNameDict.ContainsKey(name.ToLower()))
                {
                    var mapper = ConnNameDict.FirstOrDefault(x => x.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    return mapper.Value;
                }
            }

            return 0;
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            lock (ObjLock)
            {
                ConnNameDict.Clear();
            }
        }
    }
}
