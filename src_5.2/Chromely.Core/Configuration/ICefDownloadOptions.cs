// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary>
/// CEF download options.
/// </summary>
public interface ICefDownloadOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether CEF binaries should automatically be downloaded when missing.
    /// </summary>
    bool AutoDownloadWhenMissing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether CEF binaries should be downloaded silently.
    /// </summary>
    bool DownloadSilently { get; set; }
}