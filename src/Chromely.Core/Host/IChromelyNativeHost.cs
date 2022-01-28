// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

/// <summary>
/// Represents the native host.
/// </summary>
/// <remarks>
/// Each OS platform - Windows, Linux, MacOS will implement <see cref="IChromelyNativeHost"/> separately.
/// </remarks>
public interface IChromelyNativeHost : IDisposable
{
    /// <summary>
    /// Host created event.
    /// </summary>
    event EventHandler<CreatedEventArgs> HostCreated;

    /// <summary>
    /// Host moving event.
    /// </summary>
    event EventHandler<MovingEventArgs> HostMoving;

    /// <summary>
    /// Host size changed event.
    /// </summary>
    event EventHandler<SizeChangedEventArgs> HostSizeChanged;

    /// <summary>
    /// Host close event.
    /// </summary>
    event EventHandler<CloseEventArgs> HostClose;

    /// <summary>
    /// Gets the host handle.
    /// </summary>
    IntPtr Handle { get; }

    /// <summary>
    /// Host window creation function.
    /// </summary>
    void CreateWindow();

    /// <summary>
    /// Function to get the native host handle
    /// </summary>
    /// <remarks>
    /// Note that this is different from the "Handle". 
    /// Particularly used for Linux Gtk Window XID.
    /// </remarks>
    /// <returns>The native host handle of type <see cref="IntPtr"/>. </returns>
    IntPtr GetNativeHandle();

    /// <summary>
    /// Starts the host window.
    /// </summary>
    void Run();

    /// <summary>
    /// Gets the host window size.
    /// </summary>
    /// <returns>Instance of type <see cref="Size"/>.</returns>
    Size GetWindowClientSize();

    /// <summary>
    /// Gets the host window DPI scale.
    /// </summary>
    /// <returns>DPI value in float.</returns>
    float GetWindowDpiScale();

    /// <summary>
    /// Sets up the message interceptor for frameless window.
    /// </summary>
    /// <remarks>
    /// Windows ONLY.
    /// It is useful for resizing and draggging the host window.
    /// </remarks>
    /// <param name="browserWindowHandle">The browser window handle. Usually the main handle.</param>
    void SetupMessageInterceptor(IntPtr browserWindowHandle);

    /// <summary>
    /// Resize the browser, which also resizes the host window.
    /// </summary>
    /// <param name="browserWindow">The browser window handle. Usually the main handle.</param>
    /// <param name="width">The host window width.</param>
    /// <param name="height">The host window height.</param>
    void ResizeBrowser(IntPtr browserWindow, int width, int height);

    /// <summary>
    /// Function to exit the window host, the application.
    /// </summary>
    void Exit();

    /// <summary>
    /// Sets the window host title.
    /// </summary>
    /// <param name="title">The window host title.</param>
    void SetWindowTitle(string title);

    /// <summary> 
    /// Gets the current window state Maximised / Normal / Minimised etc. 
    /// </summary>
    /// <returns> The window state of instance <see cref="WindowState"/>.</returns>
    WindowState GetWindowState();

    /// <summary> 
    /// Sets window state. Maximise / Minimize / Restore. 
    /// </summary>
    /// <param name="state"> The state to set. </param>
    /// <returns> True if it succeeds, false if it fails. </returns>
    bool SetWindowState(WindowState state);

    /// <summary>
    /// Toggles window from restore state to fullscreen.
    /// </summary>
    /// <param name="hWnd">The window host handle.</param>
    void ToggleFullscreen(IntPtr hWnd);
}