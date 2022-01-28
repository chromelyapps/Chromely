// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <summary>
/// Chromely information provider class.
/// </summary>
public interface IChromelyInfo
{
    /// <summary>
    /// Get the information data.
    /// </summary>
    /// <remarks>
    /// Info includes "About Chromely", platform supported and the Chromium/CefGlue/CefSharp version.
    /// </remarks>
    /// <param name="requestId">Request identifier.</param>
    /// <returns>instance of <see cref="IChromelyResponse"/>.</returns>
    IChromelyResponse GetInfo(string requestId);

    /// <summary>
    /// Get the information data.
    /// </summary>
    /// <remarks>
    /// Info includes "About Chromely", platform supported and the Chromium/CefGlue/CefSharp version.
    /// </remarks>
    /// <returns>Map of property and information data.</returns>
    IDictionary<string, string> GetInfo();
}