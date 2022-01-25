// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

public partial class WindowController
{
    protected IntPtr _nativeHandle;

    #region

    /// <summary>
    /// On window created event handler.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="createdEventArgs">Created event arguments of <see cref="CreatedEventArgs"/> instance.</param>
    protected virtual void OnWindowCreated(object sender, CreatedEventArgs createdEventArgs)
    {
        if (createdEventArgs is not null)
        {
            _nativeHandle = createdEventArgs.Window;
            _window?.Create(_nativeHandle, createdEventArgs.WinXID);
        }
    }

    /// <summary>
    /// On window moving event handler.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="movingEventArgs">Moving event arguments of <see cref="MovingEventArgs"/> instance.</param>
    protected virtual void OnWindowMoving(object sender, MovingEventArgs movingEventArgs)
    {
        _window?.NotifyOnMove();
    }

    /// <summary>
    /// On window size changed event handler.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="sizeChangedEventArgs">Moving event arguments of <see cref="SizeChangedEventArgs"/> instance.</param>
    protected virtual void OnWindowSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
        if (sizeChangedEventArgs is not null)
        {
            _window?.Resize(sizeChangedEventArgs.Width, sizeChangedEventArgs.Height);
        }
    }

    /// <summary>
    /// On window close event handler.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="closeChangedEventArgs">Moving event arguments of <see cref="CloseEventArgs"/> instance.</param>
    protected void OnWindowClose(object sender, CloseEventArgs closeChangedEventArgs)
    {
        ChromelyAppUser.App.Properties.Save(_config);
    }

    #endregion

    protected void NativeHost_CreateAndShowWindow()
    {
        if (_config.Platform != ChromelyPlatform.Windows)
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                ActionTask.PostTask(CefThreadId.UI, NativeHost_CreateAndShowWindow);
                return;
            }
        }

        _nativeHost?.CreateWindow();
    }

    protected void NativeHost_Run()
    {
        _nativeHost?.Run();
    }

    protected void NativeHost_Quit()
    {
        _nativeHost?.Exit();
    }
}