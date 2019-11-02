using PInvoke.Net;
using System;
using System.Runtime.InteropServices;
using static Chromely.Native.WinNativeMethods;

namespace Chromely.Native
{
    public partial class WinAPIHost
    {
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookInfo = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                var key = (Keyboard.VirtualKeyStates)hookInfo.vkCode;


                bool alt = Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_MENU);
                bool control = Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_CONTROL); 

                if (!AllowKeyboardInput(alt, control, key))
                {
                    return (IntPtr)1; // Handled.
                }
            }

            return CallNextHookEx(NativeInstance._hookID, nCode, wParam, lParam);
        }

        /// <summary>Determines whether the specified keyboard input should be allowed to be processed by the system.</summary>
        /// <remarks>Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.</remarks>
        public static bool AllowKeyboardInput(bool alt, bool control, Keyboard.VirtualKeyStates key)
        {
            // Disallow various special keys.
            if (key <= Keyboard.VirtualKeyStates.VK_BACK || key == Keyboard.VirtualKeyStates.VK_NONAME ||
                key == Keyboard.VirtualKeyStates.VK_MENU || key == Keyboard.VirtualKeyStates.VK_PAUSE ||
                key == Keyboard.VirtualKeyStates.VK_HELP)
            {
                return false;
            }

            // Disallow ranges of special keys.
            // Currently leaves volume controls enabled; consider if this makes sense.
            // Disables non-existing Keys up to 65534, to err on the side of caution for future keyboard expansion.
            if ((key >= Keyboard.VirtualKeyStates.VK_LWIN && key <= Keyboard.VirtualKeyStates.VK_SLEEP) ||
                (key >= Keyboard.VirtualKeyStates.VK_KANA && key <= Keyboard.VirtualKeyStates.VK_HANJA) ||
                (key >= Keyboard.VirtualKeyStates.VK_CONVERT && key <= Keyboard.VirtualKeyStates.VK_MODECHANGE) ||
                //(key >= VirtualKey.BROWSER_BACK && key <= VirtualKey.BROWSER_HOME) ||
                (key >= Keyboard.VirtualKeyStates.VK_MEDIA_NEXT_TRACK && key <= Keyboard.VirtualKeyStates.VK_LAUNCH_APP2) ||
                (key >= Keyboard.VirtualKeyStates.VK_PROCESSKEY && key <= (Keyboard.VirtualKeyStates)65534))
            {
                return false;
            }

            // Disallow specific key combinations. (These component keys would be OK on their own.)
            if ((alt && key == Keyboard.VirtualKeyStates.VK_TAB) ||
                (alt && key == Keyboard.VirtualKeyStates.VK_SPACE) ||
                (control && key == Keyboard.VirtualKeyStates.VK_ESCAPE))
            {
                return false;
            }

            // Allow anything else (like letters, numbers, spacebar, braces, and so on).
            return true;
        }

        /// <summary>
        /// Detach the keyboard hook; call during shutdown to prevent calls as we unload
        /// </summary>
        protected static void DetachKeyboardHook()
        {
            if (NativeInstance._hookID != IntPtr.Zero)
                UnhookWindowsHookEx(NativeInstance._hookID);
        }

        internal static WinAPIHost NativeInstance { get; set; }
    }
}
