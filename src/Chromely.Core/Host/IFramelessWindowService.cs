// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

/// <summary>
/// Represents frameless window service.
/// </summary>
public interface IFramelessWindowService
{
    /// <summary>
    /// Gets the host handle.
    /// </summary>
    IntPtr Handle { get; }

    /// <summary>
    /// Closes the frameless host window.
    /// </summary>
    void Close();

    /// <summary>
    /// Maximizes the window host.
    /// </summary>
    bool Maximize();

    /// <summary>
    /// Minimizes the window host.
    /// </summary>
    bool Minimize();

    /// <summary>
    /// Restores the window host to previous state.
    /// </summary>
    bool Restore();
}