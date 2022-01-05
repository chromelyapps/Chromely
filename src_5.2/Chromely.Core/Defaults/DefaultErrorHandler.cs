// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

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

    public virtual IChromelyResource HandleError(FileInfo? fileInfo, Exception? exception = null)
    {
        var info = DefaultErrorHandler.GetFileInfo(fileInfo);
        bool fileExists = info.Item1;
        int fileSize = info.Item2;

        var resource = DefaultErrorHandler.HandleResourceError(fileExists, fileSize, exception);
        Logger.Instance.Log.LogWarning("File: {fileInfo?.FullName}: {resource?.StatusText}", fileInfo?.FullName, resource?.StatusText);
        return resource;
    }

    public IChromelyResource HandleError(Stream? stream, Exception? exception = null)
    {
        var info = DefaultErrorHandler.GetFileInfo(stream);
        bool fileExists = info.Item1;
        int fileSize = info.Item2;

        return DefaultErrorHandler.HandleResourceError(fileExists, fileSize, exception);
    }

    public virtual IChromelyResponse HandleError(IChromelyRequest request, Exception? exception = null)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
        }

        var localResponse = new ChromelyResponse
        {
            ReadyState = (int)ReadyState.ResponseIsReady,
            Status = (int)System.Net.HttpStatusCode.BadRequest,
            StatusText = "An error has occurred"
        };

        localResponse.RequestId = (request is not null && string.IsNullOrWhiteSpace(request.Id))
                                ? request.Id
                                : localResponse.RequestId;

        return localResponse;
    }

    public virtual IChromelyResponse HandleError(IChromelyRequest request, IChromelyResponse response, Exception? exception = null)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
        }

        var localResponse = new ChromelyResponse
        {
            ReadyState = (int)ReadyState.ResponseIsReady,
            Status = (int)System.Net.HttpStatusCode.BadRequest,
            StatusText = "An error has occurred"
        };

        localResponse.RequestId = (request is not null && string.IsNullOrWhiteSpace(request.Id))
                                ? request.Id
                                : localResponse.RequestId;

        return localResponse;
    }

    public virtual Task<IChromelyResource> HandleErrorAsync(string requestUrl, IChromelyResource response, Exception? exception = null)
    {
        return Task.FromResult<IChromelyResource>(response);
    }

    private static IChromelyResource HandleResourceError(bool fileExists, int fileSize, Exception? exception = null)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
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

    private static (bool, int) GetFileInfo(object? infoOrStream)
    {
        bool fileExists = false;
        int fileSize = 0;

        try
        {
            var fileInfo = infoOrStream as FileInfo;
            if (fileInfo is not null)
            {
                fileExists = fileInfo is not null && fileInfo.Exists;
                fileSize = (int)(fileInfo is not null ? fileInfo.Length : 0);
            }

            var stream = infoOrStream as Stream;
            if (stream is not null)
            {
                fileExists = stream is not null;
                fileSize = (int)(stream is not null ? stream.Length : 0);
            }

            return (fileExists, fileSize);
        }
        catch { }

        return (fileExists, fileSize);
    }
}
