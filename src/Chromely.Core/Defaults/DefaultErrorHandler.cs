// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Chromely.Core.Defaults
{
    public class DefaultErrorHandler : IChromelyErrorHandler
    {
        public virtual void HandleResourceError(string resourceStatus, string file, out HttpStatusCode statusCode, out string statusText)
        {
            HandleResourceError(resourceStatus, file, null, out statusCode, out  statusText);
        }

        public virtual void HandleResourceError(string resourceStatus, string file, Exception exception, out HttpStatusCode statusCode, out string statusText)
        {
            switch (resourceStatus?.ToUpper())
            {
                case ResourceStatus.ZeroFileSize:
                    statusCode = HttpStatusCode.NotFound;
                    statusText = "Resource loading error: file size is zero.";
                    break;

                case ResourceStatus.FileNotFound:
                    statusCode = HttpStatusCode.NotFound;
                    statusText = "File not found.";
                    break;

                case ResourceStatus.FileProcessingError:
                    statusCode = HttpStatusCode.BadRequest;
                    statusText = "Resource loading error.";
                    break;

                default:
                    statusCode = HttpStatusCode.BadRequest;
                    statusText = "Resource loading error";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(file))
            {
                Logger.Instance.Log.LogWarning($"File: {file}: {statusText}");
            }

            if (exception != null)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }
        }

        public virtual IChromelyResponse HandleRouteNotFound(string requestId, string routePath)
        {
            return new ChromelyResponse
            {
                RequestId = requestId,
                ReadyState = (int)ReadyState.ResponseIsReady,
                Status = (int)System.Net.HttpStatusCode.BadRequest,
                StatusText = $"Route for path = {routePath} is null or invalid."
            };
        }

        public virtual IChromelyResponse HandleError(IChromelyRequest request, Exception exception)
        {
            Logger.Instance.Log.LogError(exception, exception.Message);

            return new ChromelyResponse
            {
                RequestId = request?.Id,
                ReadyState = (int)ReadyState.ResponseIsReady,
                Status = (int)System.Net.HttpStatusCode.BadRequest,
                StatusText = "An error has occurred"
            };
        }

        public virtual IChromelyResponse HandleError(IChromelyRequest request, IChromelyResponse response, Exception exception)
        {
            Logger.Instance.Log.LogError(exception, exception.Message);

            return new ChromelyResponse
            {
                RequestId = request?.Id,
                ReadyState = (int)ReadyState.ResponseIsReady,
                Status = (int)System.Net.HttpStatusCode.BadRequest,
                StatusText = "An error has occurred"
            };
        }
    }
}

