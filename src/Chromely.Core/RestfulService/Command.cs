// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Command.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The command.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class using a synchronous action.
        /// </summary>
        /// <param name="path">
        /// The url.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public Command(string path, Action<IDictionary<string, string[]>> action)
        {
            Path = path;
            Key = GetKeyFromPath(path);
            Action = action;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the action async.
        /// </summary>
        public Action<IDictionary<string, string[]>> Action { get; set; }

        /// <summary>
        /// Invokes the registered action.
        /// </summary>
        /// <param name="queryParameters">
        /// The request query parameters.
        /// </param>
        public void Invoke(IDictionary<string, string[]> queryParameters)
        {
             Action.Invoke(queryParameters);
        }

        public static string GetKeyFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            return $"{path}".Replace("/", "_").ToLower();
        }
    }
}
