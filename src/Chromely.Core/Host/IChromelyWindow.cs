// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

/// <summary>
/// Represents the window host.
/// </summary>
public interface IChromelyWindow : IDisposable
{
    /// <summary>
    /// Gets the host handle.
    /// </summary>
    IntPtr Handle { get; }

    /// <summary>
    /// Gets the native host - <see cref="IChromelyNativeHost"/> instance.
    /// </summary>
    IChromelyNativeHost NativeHost { get; }

    /// <summary>
    /// Initializes the window using a customizable set of settings.
    /// </summary>
    /// <param name="settings">Customizable set of settings.</param>
    void Init(object settings);

    /// <summary>
    /// Creates the window host.
    /// </summary>
    /// <param name="hostHandle">The host handle.</param>
    /// <param name="winXID">The native host Window XID.</param>
    void Create(IntPtr hostHandle, IntPtr winXID);

    /// <summary>
    /// Closes the host window.
    /// </summary>
    void Close();

    /// <summary>
    /// Sets the host window title.
    /// </summary>
    /// <param name="title">The window host title.</param>
    void SetTitle(string title);

    /// <summary>
    /// Resizes the host window.
    /// </summary>
    /// <param name="width">The host window width.</param>
    /// <param name="height">The host window height.</param>
    void Resize(int width, int height);

    /// <summary>
    /// Registers windows handlers.
    /// </summary>
    /// <remarks>
    /// The function can be used to register handlers for keyboard, mouse, hot keys functionalities, etc.
    /// </remarks>
    void RegisterHandlers();

    /// <summary>
    /// Function to call when browser is moved.
    /// </summary>
    void NotifyOnMove();

    /// <summary>
    /// Minimizes the window host.
    /// </summary>
    void Minimize();

    /// <summary>
    /// Maximizes the window host.
    /// </summary>
    void Maximize();

    /// <summary>
    /// Restores the window host to previous state.
    /// </summary>
    void Restore();
}