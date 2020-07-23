// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using System;
using System.Runtime.InteropServices;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    public class KeyboardLLHook : WindowsHookBase
	{
        protected IWindowOptions _options;

        public KeyboardLLHook(IWindowOptions options) : base(WH.KEYBOARD_LL)
		{
            _options = options;
            HookEventHandler = OnKeyboardEvent;
		}

		protected virtual bool OnKeyboardEvent(HookEventArgs args)
		{
			if (args == null)
			{
				return false;
			}

			WM wParam = (WM)args.wParam.ToInt32();
			var hookInfo = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(args.lParam);
			var key = (Keys)hookInfo.vkCode;


			bool alt = IsKeyPressed(Keys.Menu);
			bool control = IsKeyPressed(Keys.ControlKey);

			if (!AllowKeyboardInput(alt, control, (int)key))
			{
				return true;
			}

			return false;
		}

		/// <summary>Determines whether the specified keyboard input should be allowed to be processed by the system.</summary>
		/// <remarks>Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.</remarks>
		protected virtual bool AllowKeyboardInput(bool alt, bool control, int intKey)
		{
			if (_options.KioskMode)
			{
				return AllowKeyboardInputForKioskMode(alt, control, intKey);
			}

			return true;
		}

        #region

        private bool AllowKeyboardInputForKioskMode(bool alt, bool control, int intKey)
        {
            if (!_options.KioskMode)
            {
                return true;
            }

            Keys key = (Keys)intKey;

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
}
