// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <summary>
/// Sets of error handling functions.
/// </summary>
public interface IChromelyErrorHandler
{
    /// <summary>
    /// Handle route path not found error.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="routePath">The controller route path.</param>
    /// <returns>Instance of <see cref="IChromelyResponse"/>.</returns>
    IChromelyResponse HandleRouteNotFound(string requestId, string routePath);

    /// <summary>
    /// Handle resource request error.
    /// </summary>
    /// <param name="fileInfo">The resource file - instance of <see cref="FileInfo"/>. </param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromelyResponse"/>.</returns>
    IChromelyResource HandleError(FileInfo? fileInfo, Exception? exception = null);

    /// <summary>
    /// Handle resource request error.
    /// </summary>
    /// <param name="stream">The resource file - instance of <see cref="Stream"/>. </param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromelyResponse"/>.</returns>
    IChromelyResource HandleError(Stream? stream, Exception? exception = null);

    /// <summary>
    /// Handle request error.
    /// </summary>
    /// <param name="request">The request - instance of <see cref="IChromelyRequest"/>.</param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromelyResponse"/>.</returns>
    IChromelyResponse HandleError(IChromelyRequest request, Exception? exception = null);

    /// <summary>
    /// Handle request error.
    /// </summary>
    /// <param name="request">The request - instance of <see cref="IChromelyRequest"/>.</param>
    /// <param name="response">The response - instance of <see cref="IChromelyResponse"/>.</param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromelyResponse"/>.</returns>
    IChromelyResponse HandleError(IChromelyRequest request, IChromelyResponse response, Exception? exception = null);

    /// <summary>
    /// Handle request error asynchronously.
    /// </summary>
    /// <param name="requestUrl">The controller route url path.</param>
    /// <param name="response">The response - instance of <see cref="IChromelyResponse"/>.</param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromelyResponse"/>.</returns>
    Task<IChromelyResource> HandleErrorAsync(string requestUrl, IChromelyResource response, Exception? exception = null);
}
