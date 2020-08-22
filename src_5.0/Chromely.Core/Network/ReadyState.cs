// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadyState.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Network
{
    /// <summary>
    /// The ready state.
    /// </summary>
    public enum ReadyState
    {
        /// <summary>
        /// The not initialized - request not initialized 
        /// </summary>
        NotInitialized = 0,

        /// <summary>
        /// The server connection established.
        /// </summary>
        ServerConnectionEstablished = 1,

        /// <summary>
        /// The request received.
        /// </summary>
        RequestReceived = 2,

        /// <summary>
        /// The processing request.
        /// </summary>
        ProcessingRequest = 3,

        /// <summary>
        /// The response is ready - Request finished and response is ready
        /// </summary>
        ResponseIsReady = 4
    }
}
