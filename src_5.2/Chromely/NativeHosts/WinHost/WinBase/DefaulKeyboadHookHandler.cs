// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

/// <summary>
/// Default implementation of keyboard handler of type <see cref="IKeyboadHookHandler"/>.
/// </summary>
public class DefaulKeyboadHookHandler : IKeyboadHookHandler
{
    protected IChromelyNativeHost? _nativeHost;
    protected IWindowOptions _options;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaulKeyboadHookHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    public DefaulKeyboadHookHandler(IChromelyConfiguration config)
    {
        _options = config?.WindowOptions ?? new WindowOptions();
    }

    /// <inheritdoc/>
    public void SetNativeHost(IChromelyNativeHost nativeHost)
    {
        _nativeHost = nativeHost;
    }

    /// <inheritdoc/>
    public bool HandleKey(IntPtr hWnd, object param)
    {
        if (param is not KeyboardParam keyboardParam)
        {
            return false;
        }

        if (!AllowKeyboardInput(keyboardParam.Alt, keyboardParam.Control, keyboardParam.Key))
        {
            return true;
        }

        switch (keyboardParam.Key)
        {
            case Keys.F4:
                {
                    if (!_options.WindowFrameless && _options.KioskMode)
                    {
                        return true;
                    }
                    break;
                }
            case Keys.F11:
                {
                    if (!_options.WindowFrameless && !_options.KioskMode && keyboardParam.IsKeyUp)
                    {
                        _nativeHost?.ToggleFullscreen(hWnd);
                        return true;
                    }
                    break;
                }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified keyboard input should be allowed to be processed by the OS.
    /// </summary>
    /// <remarks>
    /// Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.
    /// </remarks>
    /// <param name="alt">ALT key.</param>
    /// <param name="control">CTRL key.</param>
    /// <param name="key">Key as represented by <see cref="Keys"/>.</param>
    /// <returns>true if keyboard input can be processed, otherwise false.</returns>
    protected virtual bool AllowKeyboardInput(bool alt, bool control, Keys key)
    {
        if (_options.KioskMode)
        {
            return AllowKeyboardInputForKioskMode(alt, control, key);
        }

        return true;
    }

    #region

    /// <summary>
    /// Determines whether the specified keyboard input should be allowed to be processed by the OS in Kiosk mode.
    /// </summary>
    /// <remarks>
    /// Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.
    /// </remarks>
    /// <param name="alt">ALT key.</param>
    /// <param name="control">CTRL key.</param>
    /// <param name="key">Key as represented by <see cref="Keys"/>.</param>
    /// <returns>true if keyboard input can be processed, otherwise false.</returns>
    protected virtual bool AllowKeyboardInputForKioskMode(bool alt, bool control, Keys key)
    {
        if (!_options.KioskMode)
        {
            return true;
        }

        // Disallow various special keys.
        if (key < Keys.Back || key == Keys.NoName ||
            key == Keys.Menu || key == Keys.Pause ||
            key == Keys.Help)
        {
            return false;
        }

        // Disallow ranges of special keys.
        // Currently leaves volume controls enabled; consider if this makes sense.
        // Disables non-existing Keys up to 65534, to err on the side of caution for future keyboard expansion.
        if ((key >= Keys.LWin && key <= Keys.Sleep) ||
            (key >= Keys.KanaMode && key <= Keys.HanjaMode) ||
            (key >= Keys.IMEConvert && key <= Keys.IMEModeChange) ||
            //(key >= VirtualKey.BROWSER_BACK && key <= VirtualKey.BROWSER_HOME) ||
            (key >= Keys.MediaNextTrack && key <= Keys.LaunchApplication2) ||
            (key >= Keys.ProcessKey && key <= (Keys)65534))
        {
            return false;
        }

        // Disallow specific key combinations. (These component keys would be OK on their own.)
        if ((alt && key == Keys.Tab) ||
            (alt && key == Keys.Space) ||
            (control && key == Keys.Escape))
        {
            return false;
        }

        // Allow anything else (like letters, numbers, spacebar, braces, and so on).
        return true;
    }

    #endregion
}

#region Keyboard param
internal class KeyboardParam
{
    public KeyboardParam(bool isKeyUp, bool alt, bool control, Keys key)
    {
        IsKeyUp = isKeyUp;
        Alt = alt;
        Control = control;
        Key = key;
    }
    public bool IsKeyUp { get; set; }
    public bool Alt { get; set; }
    public bool Control { get; set; }
    public Keys Key { get; set; }
}

#endregion