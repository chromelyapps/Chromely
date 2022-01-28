// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Loader;

/// <inheritdoc/>
public class DefaultCefDownloader : ICefDownloader
{
    protected readonly IChromelyConfiguration _config;
    protected ICefDownloadNotification _notification;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultCefDownloader"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    public DefaultCefDownloader(IChromelyConfiguration config)
    {
        _config = config;
        _notification = null;
    }

    /// <inheritdoc/>
    public ICefDownloadNotification Notification
    {
        get
        {
            if (_notification is null)
            {
                switch (_config.CefDownloadOptions.NotificationType)
                {
                    case CefDownloadNotificationType.Logger:
                        _notification = new LoggerCefDownloadNotification();
                        break;

                    case CefDownloadNotificationType.Console:
                        _notification = new ConsoleCefDownloadNotification();
                        break;

                    case CefDownloadNotificationType.HTML:
                        _notification = new HtmlCefDownloadNotification();
                        break;

                    default:
                        _notification = new LoggerCefDownloadNotification();
                        break;
                }
            }

            return _notification;
        }
    }

    /// <inheritdoc/>
    public void Download(IChromelyConfiguration config)
    {
        CefLoader.Download(config.Platform);
    }
}