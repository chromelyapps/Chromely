// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Chromely.Interops;

namespace Chromely
{
    public static partial class Interop
    {
        public static partial class User32
        {
            [DllImport(Libraries.User32)]
            static extern short GetKeyState(Keys nVirtKey);

            [StructLayout(LayoutKind.Sequential)]
            public struct MSLLHOOKSTRUCT
            {
                public POINT pt;
                public int mouseData;
                public int flags;
                public int time;
                public UIntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            public class KBDLLHOOKSTRUCT
            {
                public uint vkCode;
                public uint scanCode;
                public KBDLLHOOKSTRUCTFlags flags;
                public uint time;
                public UIntPtr dwExtraInfo;
            }

            [Flags]
            public enum KBDLLHOOKSTRUCTFlags : uint
            {
                LLKHF_EXTENDED = 0x01,
                LLKHF_INJECTED = 0x10,
                LLKHF_ALTDOWN = 0x20,
                LLKHF_UP = 0x80,
            }

            public static IntPtr SetHook(HOOKPROC proc)
            {
                using (var curProcess = Process.GetCurrentProcess())
                {
                    using (var curModule = curProcess.MainModule)
                    {
                        return SetWindowsHookExW(WH.KEYBOARD_LL, proc, Kernel32.GetModuleHandleW(curModule.ModuleName), 0);
                    }
                }
            }

            public static IntPtr SetHook(WH hookType, HOOKPROC proc)
            {
                using (var curProcess = Process.GetCurrentProcess())
                {
                    using (var curModule = curProcess.MainModule)
                    {
                        return SetWindowsHookExW(hookType, proc, Kernel32.GetModuleHandleW(curModule.ModuleName), 0);
                    }
                }
            }

            public static bool IsKeyPressed(uint testKey)
            {
                return IsKeyPressed((Keys)testKey);
            }

            public static bool IsKeyPressed(Keys testKey)
            {
                short result = GetKeyState(testKey);

                bool keyPressed;
                switch (result)
                {
                    case 0:
                        // Not pressed and not toggled on.
                        keyPressed = false;
                        break;

                    case 1:
                        // Not pressed, but toggled on
                        keyPressed = false;
                        break;

                    default:
                        // Pressed (and may be toggled on)
                        keyPressed = true;
                        break;
                }

                return keyPressed;
            }
        }
    }
}
