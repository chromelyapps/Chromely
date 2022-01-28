// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <summary>
/// Represents a utility wrapper for executing JavaScript scripts in the browser.
/// </summary>
public interface IChromelyJavaScriptExecutor
{
    /// <summary>
    /// Execute JavaScript script.
    /// </summary>
    /// <param name="frameName">The frame name on which the script is to be executed.</param>
    /// <param name="script">The script to execute.</param>
    /// <returns>ignored.</returns>
    object ExecuteScript(string frameName, string script);

    /// <summary>
    /// Execute JavaScript script.
    /// </summary>
    /// <remarks>
    /// No frame name specified, so script will run on main frame.
    /// </remarks>
    /// <param name="script">The script to execute.</param>
    /// <returns>ignored.</returns>
    object ExecuteScript(string script);

    /// <summary>
    /// Gets the browser object.
    /// </summary>
    /// <returns></returns>
    object? GetBrowser();

    /// <summary>
    /// Gets the the main frame.
    /// </summary>
    /// <returns></returns>
    object? GetMainFrame();

    /// <summary>
    /// Gets a frame object by frame name.
    /// </summary>
    /// <param name="frameName">The frame name.</param>
    /// <returns>Frame object.</returns>
    object? GetFrame(string frameName);

    /// <summary>
    /// Get all frame identifiers.
    /// </summary>
    List<long> GetFrameIdentifiers { get; }

    /// <summary>
    /// Get all frame names.
    /// </summary>
    List<string> GetFrameNames { get; }
}