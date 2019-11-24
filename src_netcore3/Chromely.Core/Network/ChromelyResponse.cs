// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyResponse.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Network
{
    /// <summary>
    /// The chromely response.
    /// </summary>
    public class ChromelyResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyResponse"/> class.
        /// </summary>
        public ChromelyResponse()
        {
            RequestId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyResponse"/> class.
        /// </summary>
        /// <param name="requestId">
        /// The request id.
        /// </param>
        public ChromelyResponse(string requestId)
        {
            RequestId = requestId;
        }

        /// <summary>
        /// Gets or sets the route path.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the ready state.
        /// </summary>
        public int ReadyState { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public object Data { get; set; }
    }
}
