// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionNameMapper.cs" company="Chromely">
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

namespace Chromely.CefGlue.Winapi.Browser.ServerHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Chromely.Core.RestfulService;

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
