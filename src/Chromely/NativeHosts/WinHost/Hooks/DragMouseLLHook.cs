// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using static Chromely.Interop;
using static Chromely.Interop.User32;

namespace Chromely.NativeHost
{
	public class DragMouseLLHook : WindowsHookBase
	{
		protected DragWindowInfo _dragWindowInfo;

		/// <summary>
		/// Occurs when a key is pressed while keyboard lockdown is engaged.
		/// </summary>
		public static event EventHandler<MouseMoveEventArgs> MouseMoveHandler;
		public static event EventHandler<LeftButtonUpEventArgs> LeftButtonUpHandler;

		public DragMouseLLHook(DragWindowInfo dragWindowInfo) : base(WH.MOUSE_LL)
		{
			_dragWindowInfo = dragWindowInfo;
			HookEventHandler = OnMouseEvent;
		}

		protected virtual bool OnMouseEvent(HookEventArgs args)
		{
			if (args == null)
			{
				return false;
			}

			WM wParam = (WM)args.wParam.ToInt32();
			switch (wParam)
			{
				case WM.MOUSEMOVE:
					if (_dragWindowInfo != null && _dragWindowInfo.DragInitiated)
					{
						POINT cursorPos = new POINT();
						if (GetCurrentCursorLocation(args.lParam, ref cursorPos))
						{
							var ptDelta = new POINT(cursorPos.x - _dragWindowInfo.DragPoint.x, cursorPos.y - _dragWindowInfo.DragPoint.y);
							var newLocation = new POINT(_dragWindowInfo.DragWindowLocation.x + ptDelta.x, _dragWindowInfo.DragWindowLocation.y + ptDelta.y);
							OnMouseMove(newLocation.x, newLocation.y);

							return false;
						}
					}
					break;

				// button up messages
				case WM.LBUTTONUP:
					if (_dragWindowInfo != null && _dragWindowInfo.DragInitiated)
					{
						OnLeftButtonUp();
					}
					break;
			}

			return false;
		}

		private bool GetCurrentCursorLocation(IntPtr lParam, ref POINT cursorPos)
		{
			try
			{
				var hookInfo = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
				cursorPos = hookInfo.pt;
				return true;
			}
			catch {}

			return false;
		}

		#region

		static void OnMouseMove(int deltaX, int deltaY)
		{
			var handler = MouseMoveHandler;
			if (handler != null)
			{
				var args = new MouseMoveEventArgs(deltaX, deltaY);
				handler.Invoke(null, args);
			}
		}

		static void OnLeftButtonUp()
		{
			var handler = LeftButtonUpHandler;
			if (handler != null)
			{
				var args = new LeftButtonUpEventArgs();
				handler.Invoke(null, args);
			}
		}

		#endregion
	}
}
