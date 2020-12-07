using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    internal class DefaulKeyboadHookHandler : IKeyboadHookHandler
    {
        protected IChromelyNativeHost _nativeHost;
        protected IWindowOptions _options;

        public DefaulKeyboadHookHandler(IChromelyConfiguration config)
        {
            _options = config?.WindowOptions ?? new WindowOptions();
        }

        public void SetNativeHost(IChromelyNativeHost nativeHost)
        {
            _nativeHost = nativeHost;
        }

        public bool HandleKey(IntPtr hWnd, object param)
        {
            KeyboardParam keyboardParam = param as KeyboardParam;
            if (keyboardParam == null)
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

        /// <summary>Determines whether the specified keyboard input should be allowed to be processed by the system.</summary>
        /// <remarks>Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.</remarks>
        private bool AllowKeyboardInput(bool alt, bool control, Keys key)
        {
            if (_options.KioskMode)
            {
                return AllowKeyboardInputForKioskMode(alt, control, key);
            }

            return true;
        }

        #region

        private bool AllowKeyboardInputForKioskMode(bool alt, bool control, Keys key)
        {
            if (!_options.KioskMode)
            {
                return true;
            }

            // Disallow various special keys.
            if (key <= Keys.Back || key == Keys.NoName ||
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
}
