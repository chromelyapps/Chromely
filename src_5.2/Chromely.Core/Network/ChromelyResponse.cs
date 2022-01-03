// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core.Network
{
    /// <summary>
    /// The Chromely response.
    /// </summary>
    public class ChromelyResponse : IChromelyResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyResponse"/> class.
        /// </summary>
        public ChromelyResponse()
        {
            RequestId = Guid.NewGuid().ToString();
            Error = string.Empty;
            Status = ResponseConstants.StatusOK;
            StatusText = ResponseConstants.StatusOKText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyResponse"/> class.
        /// </summary>
        /// <param name="requestId">
        /// The request id.
        /// </param>
        public ChromelyResponse(string requestId)
            : this()
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
        public object? Data { get; set; }

        public bool HasRouteResponse { get; set; }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Error) || Status != (int)System.Net.HttpStatusCode.OK;
            }
        }

        public string Error { get; set; }
    }
}
