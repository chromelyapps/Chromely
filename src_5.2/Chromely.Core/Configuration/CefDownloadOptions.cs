// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary>
/// <see cref="CefDownloadOptions"/> allows setting download options.
/// </summary>
public class CefDownloadOptions : ICefDownloadOptions
{
    /// <summary>
    /// Initializes a new instance of <see cref="CefDownloadOptions"/>.
    /// </summary>
    public CefDownloadOptions()
    {
        AutoDownloadWhenMissing = true;
        DownloadSilently = false;
        NotificationType = CefDownloadNotificationType.Logger;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CefDownloadOptions"/>.
    /// </summary>
    /// <param name="autoDownload">Flag to set if auto download is allowed.</param>
    /// <param name="silentDownload">Flag to set to download silently.</param>
    /// <param name="notificationType">Instance of <see cref="CefDownloadNotificationType"/>.</param>
    public CefDownloadOptions(bool autoDownload, 
                             bool silentDownload, 
                             CefDownloadNotificationType notificationType = CefDownloadNotificationType.Logger)
    {
        AutoDownloadWhenMissing = autoDownload;
        DownloadSilently = silentDownload;
        NotificationType = notificationType;
    }

    /// <summary>
    /// Gets or sets a value indicating whether CEF binaries should automatically be downloaded when missing.
    /// </summary>
    public bool AutoDownloadWhenMissing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether CEF binaries should be downloaded silently.
    /// </summary>
    public bool DownloadSilently { get; set; }

    /// <summary>
    /// Gets or sets the binaries download notification type.
    /// </summary>
    public CefDownloadNotificationType NotificationType { get; set; }
}