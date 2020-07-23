// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
	public abstract class WindowsHookBase : IDisposable
	{
		public Func<HookEventArgs, bool> HookEventHandler;
	
		protected IntPtr _hookID = IntPtr.Zero;
		protected HOOKPROC _filterFunc = null;
		protected WH _hookType;

		private bool _disposed = false;

		public WindowsHookBase(WH hook)
		{
			_hookID = IntPtr.Zero;
			_hookType = hook;
			_filterFunc = new HOOKPROC(this.CoreHookProc);
		}
		public WindowsHookBase(WH hook, HOOKPROC func)
		{
			_hookID = IntPtr.Zero;
			_hookType = hook;
			_filterFunc = func;
		}

		protected IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
		{
			if (code < 0)
			{
				return CallNextHookEx(_hookID, (HC)code, wParam, lParam);
			}

			if (code == (int)HC.ACTION)
			{
				// Let clients determine what to do
				var handler = HookEventHandler;
				HookEventArgs e = new HookEventArgs();
				e.HookCode = code;
				e.wParam = wParam;
				e.lParam = lParam;

				if (handler != null)
				{
					bool handled = handler.Invoke(e);
					if (handled)
					{
						return (IntPtr)1; // Handled.
					}
				}
			}

			// Yield to the next hook in the chain
			return CallNextHookEx(_hookID, (HC)code, wParam, lParam);
		}

		public void Install()
		{
			_hookID = SetHook(_hookType, _filterFunc);
		}

		public void Uninstall()
		{
			UnhookWindowsHookEx(_hookID);
			_hookID = IntPtr.Zero;
		}

		public bool IsInstalled
		{
			get { return _hookID != IntPtr.Zero; }
		}

		#region Disposal

		~WindowsHookBase()
		{
			Dispose(false);
		}

		protected void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			// If there are managed resources
			if (disposing)
			{
			}

			if (IsInstalled)
				Uninstall();

			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
