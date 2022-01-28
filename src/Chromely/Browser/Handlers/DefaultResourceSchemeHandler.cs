// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default implementation of <see cref="ResourceHandler"/>. 
/// At a minimum developer only need to override ProcessRequestAsync.
/// </summary>
public class DefaultResourceSchemeHandler : ResourceHandler
{
    protected readonly IChromelyConfiguration _config;
    protected readonly IChromelyErrorHandler _chromelyErrorHandler;
    protected IChromelyResource _chromelyResource;
    protected FileInfo? _fileInfo;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultResourceSchemeHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="chromelyErrorHandler">Instance of <see cref="IChromelyErrorHandler"/>.</param>
    public DefaultResourceSchemeHandler(IChromelyConfiguration config, IChromelyErrorHandler chromelyErrorHandler)
    {
        _config = config;
        _chromelyErrorHandler = chromelyErrorHandler;
        _chromelyResource = new ChromelyResource();
        _fileInfo = null;
    }

    /// <summary>
    /// The process request async.
    /// </summary>
    /// <param name="request">
    /// The request.
    /// </param>
    /// <param name="callback">
    /// The callback.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
    {
        var uri = new Uri(request.Url);
        var scheme = _config.UrlSchemes.GetScheme(request.Url);
        var isFolderResourceScheme = scheme is not null && scheme.IsUrlSchemeFolderResource();

        var u = new Uri(request.Url);
        var file = isFolderResourceScheme
                    ? scheme.GetResourceFolderFile(u.AbsolutePath)
                    : u.Authority + u.AbsolutePath;

        _fileInfo = new FileInfo(file);
        // Check if file exists 
        if (!_fileInfo.Exists)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo);
            callback.Continue();
        }
        // Check if file exists but empty
        else if (_fileInfo.Length == 0)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo);
            callback.Continue();
        }
        else
        {
            Task.Run(() =>
            {
                using (callback)
                {
                    _chromelyResource.Content = null;
                    _chromelyResource.MimeType = "text/html";

                    try
                    {
                        byte[] fileBytes = File.ReadAllBytes(file);
                        _chromelyResource.Content = new MemoryStream(fileBytes);

                        string extension = Path.GetExtension(file);
                        _chromelyResource.MimeType = MimeMapper.GetMimeType(extension);
                        _chromelyResource.StatusCode = ResourceConstants.StatusOK;
                        _chromelyResource.StatusText = ResourceConstants.StatusOKText;
                    }
                    catch (Exception exception)
                    {
                        _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo, exception);
                    }

                    if (_chromelyResource.Content is null)
                    {
                        callback.Cancel();
                    }
                    else
                    {
                        SetResponseInfoOnSuccess();
                        callback.Continue();
                    }
                }
            });
        }

        return CefReturnValue.ContinueAsync;
    }

    protected virtual void SetResponseInfoOnSuccess()
    {
        Stream = Stream.Null;

        //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
        if (_chromelyResource.Content is not null)
        {
            _chromelyResource.Content.Position = 0;
            //Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
            ResponseLength = _chromelyResource.Content.Length;
            Stream = _chromelyResource.Content;
        }

        MimeType = _chromelyResource.MimeType;
        StatusCode = (int)_chromelyResource.StatusCode;
        StatusText = _chromelyResource.StatusText;
    }
}