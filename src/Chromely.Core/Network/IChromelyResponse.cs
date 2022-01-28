// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

/// <summary>
/// The Chromely response from invoking controller route actions.
/// </summary>
public interface IChromelyResponse
{
    /// <summary>
    /// Gets or sets the route path.
    /// </summary>
    string RequestId { get; set; }

    /// <summary>
    /// Gets or sets the ready state.
    /// </summary>
    int ReadyState { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    int Status { get; set; }

    /// <summary>
    /// Gets or sets the status text.
    /// </summary>
    string StatusText { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    object? Data { get; set; }

    /// <summary>
    /// Gets a value indicating whether response has an error.
    /// </summary>
    bool HasError { get; }

    /// <summary>
    /// Gets or sets the error message. 
    /// </summary>
    string Error { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether controller route action has a response.
    /// </summary>
    /// <remarks>
    /// Samples:
    /// Has response: int GetId(){}
    /// No response: void SetId(int id){}
    /// </remarks>
    bool HasRouteResponse { get; set; }
}
