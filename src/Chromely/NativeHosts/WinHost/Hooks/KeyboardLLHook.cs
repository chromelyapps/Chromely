// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System;
using System.Runtime.InteropServices;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
    public class KeyboardLLHook : WindowsHookBase
	{
        protected IntPtr _handler;
        protected IWindowOptions _options;
        protected IKeyboadHookHandler _keyboadHandler;

        public KeyboardLLHook(IntPtr handler, IWindowOptions options, IKeyboadHookHandler keyboadHandler) : base(WH.KEYBOARD_LL)
		{
            _handler = handler;
            _options = options;
            _keyboadHandler = keyboadHandler;
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

            return _keyboadHandler.HandleKey(_handler, new KeyboardParam(wParam == WM.KEYUP,  alt, control, key));
		}
    }
}
