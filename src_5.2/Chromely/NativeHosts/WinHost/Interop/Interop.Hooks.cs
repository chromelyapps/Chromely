// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

namespace Chromely;

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
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            return SetWindowsHookExW(WH.KEYBOARD_LL, proc, Kernel32.GetModuleHandleW(curModule.ModuleName), 0);
        }

        public static IntPtr SetHook(WH hookType, HOOKPROC proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            return SetWindowsHookExW(hookType, proc, Kernel32.GetModuleHandleW(curModule.ModuleName), 0);
        }

        public static bool IsKeyPressed(uint testKey)
        {
            return IsKeyPressed((Keys)testKey);
        }

        public static bool IsKeyPressed(Keys testKey)
        {
            short result = GetKeyState(testKey);
            var
                // Not pressed and not toggled on.
                keyPressed = result switch
                {
                    0 => false,// Not pressed and not toggled on.
                        1 => false,// Not pressed, but toggled on
                        _ => true,// Pressed (and may be toggled on)
                    };
            return keyPressed;
        }
    }
}