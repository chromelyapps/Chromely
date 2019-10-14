// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyMessageRouter.cs" company="Chromely Projects">
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
    /// The chromely messsage router.
    /// Registers MessageRouter handler -
    /// https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage.md#markdown-header-generic-message-router
    /// </summary>
    public class ChromelyMessageRouter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyMessageRouter"/> class.
        /// </summary>
        public ChromelyMessageRouter()
        {
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyMessageRouter"/> class.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        public ChromelyMessageRouter(object handler)
        {
            Key = Guid.NewGuid().ToString();
            Handler = handler;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the handler.
        /// </summary>
        public object Handler { get; set; }
    }
}
