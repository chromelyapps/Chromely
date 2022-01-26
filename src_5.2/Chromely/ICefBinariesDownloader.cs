// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <summary>
/// Represents CEF binaries downloader.
/// </summary>
public interface ICefBinariesDownloader
{
    /// <summary>
    /// Primary entry function for downloading CEF binaries.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    void Download(IChromelyConfiguration config);
}