// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyEventHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core.Helpers;

namespace Chromely.Core
{
    /// <summary>
    /// The chromely event handler.
    /// </summary>
    /// <typeparam name="T">
    /// Handler type. 
    /// </typeparam>
    public class ChromelyEventHandler<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyEventHandler{T}"/> class. 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ChromelyEventHandler()
        {
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyEventHandler{T}"/> class.
        /// </summary>
        /// <param name="eventKey">
        /// The event key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public ChromelyEventHandler(CefEventKey eventKey, EventHandler<T> handler)
        {
            Key = eventKey.EnumToString();
            Handler = handler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyEventHandler{T}"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public ChromelyEventHandler(string key, EventHandler<T> handler)
        {
            Key = key;
            Handler = handler;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        public EventHandler<T> Handler { get; set; }
    }
}
