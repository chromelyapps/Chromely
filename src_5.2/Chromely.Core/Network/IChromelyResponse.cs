// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network
{
    /// <summary>
    /// The Chromely response.
    /// </summary>
    public interface IChromelyResponse
    {
        string RequestId { get; set; }

        int ReadyState { get; set; }

        int Status { get; set; }

        string StatusText { get; set; }

        object? Data { get; set; }

        bool HasError { get; }

        string Error { get; set; }

        bool HasRouteResponse { get; set; }
    }
}
