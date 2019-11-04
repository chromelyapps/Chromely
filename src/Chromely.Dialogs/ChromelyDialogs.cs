using System;
using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Dialogs.Linux;
using Chromely.Dialogs.Windows;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Chromely.Dialogs
{
    public static class ChromelyDialogs
    {
        private static readonly IChromelyDialogs Dialogs;

        static ChromelyDialogs()
        {
            switch (ChromelyRuntime.Platform)
            {
                case ChromelyPlatform.Windows:
                    Dialogs = new WindowsDialogs();
                    return;
                case ChromelyPlatform.Linux:
                    Dialogs = new LinuxDialogs();
                    return;
                case ChromelyPlatform.MacOSX:
                    break;
            }
            throw new System.NotImplementedException();
        }

        public static void Init(IChromelyWindow window)
        {
            Dialogs.Init(window);
        }

        public static DialogResponse MessageBox(string message)
        {
            return MessageBox(message, new DialogOptions());
        }
        public static DialogResponse MessageBox(string message, DialogOptions options)
        {
            return Dialogs.MessageBox(message, options);
        }

        public static DialogResponse SelectFolder(string message, FileDialogOptions options)
        {
            return Dialogs.SelectFolder(message, options);
        }

        public static DialogResponse FileOpen(string message, FileDialogOptions options)
        {
            return Dialogs.FileOpen(message, options);
        }

        public static DialogResponse FileSave(string message, string fileName, FileDialogOptions options)
        {
            return Dialogs.FileSave(message, fileName, options);
        }
        
    }
}