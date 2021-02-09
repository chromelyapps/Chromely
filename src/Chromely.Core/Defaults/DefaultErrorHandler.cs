// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;

namespace Chromely.Core.Defaults
{
    public class DefaultErrorHandler : IChromelyErrorHandler
    {
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

        public virtual IChromelyResource HandleError(FileInfo fileInfo, Exception exception = null)
        {
            bool fileExists = fileInfo != null && fileInfo.Exists;
            int fileSize = (int)(fileInfo != null ? fileInfo.Length : 0);
            
            var resource = HandleResourceError(fileExists, fileSize, exception);
            Logger.Instance.Log.LogWarning($"File: {fileInfo?.FullName}: {resource?.StatusText}");
            return resource;
        }

        public IChromelyResource HandleError(Stream stream, Exception exception = null)
        {
            bool fileExists = stream != null;
            int fileSize = (int)(stream != null ? stream.Length : 0);

            return HandleResourceError(fileExists, fileSize, exception);
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

        private IChromelyResource HandleResourceError(bool fileExists, int fileSize, Exception exception = null)
        {
            if (exception != null)
            {
                Logger.Instance.Log.LogError(exception, exception.Message);
            }

            var resource = new ChromelyResource();
            if (!fileExists)
            {
                resource.StatusCode = HttpStatusCode.NotFound;
                resource.StatusText = "Resource loading error: file size is zero.";
                resource.Content = resource.StatusText.GetMemoryStream();
            }

            else if (fileSize == 0)
            {
                resource.StatusCode = HttpStatusCode.NotFound;
                resource.StatusText = "Resource loading error: file size is zero.";
                resource.Content = resource.StatusText.GetMemoryStream();
            }
            else
            {
                resource.StatusCode = HttpStatusCode.BadRequest;
                resource.StatusText = "Resource loading error";
                resource.Content = resource.StatusText.GetMemoryStream();
            }

            return resource;
        }
    }
}

