/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.ChromeHosts.Winapi
{
    using System;
    using WinApi.Windows;

    public static class HostHelpers
    {
        static HostHelpers()
        {
            DefaultUnhandledExceptionHandler =
                (sender, eventArgs) =>
                {
                    ShowCriticalError(eventArgs.ExceptionObject);
                    if (eventArgs.IsTerminating)
                    {
                        ShowCriticalInfo(
                            $"{Environment.NewLine}The application will now terminate. Press any key to do so.");
                        Console.ReadKey();
                    }
                };

            DefaultWindowExceptionHandler = ex =>
            {
                ShowCriticalError(ex.InnerException);
                ex.SetHandled();
            };
        }

        public static WindowExceptionHandler DefaultWindowExceptionHandler { get; }
        public static UnhandledExceptionEventHandler DefaultUnhandledExceptionHandler { get; }

        public static void SetupDefaultExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += DefaultUnhandledExceptionHandler;
            WindowCore.UnhandledException += DefaultWindowExceptionHandler;
        }

        public static void ShowCriticalError(object exceptionObj, string message = null, string title = null)
        {
            const string defaultMessage = "Oh snap! Something went wrong.";
            if (message == null) message = defaultMessage;

            // ConsoleHelpers.EnsureConsole();
            if (!string.IsNullOrEmpty(title)) Console.Title = title;
            var exception = exceptionObj as Exception;
            string msg;
            if (exception != null)
            {
                msg = $"{Environment.NewLine}[CRITICAL] {message}{Environment.NewLine}{Environment.NewLine}" +
                      $"{exception.Message}{Environment.NewLine}" +
                      $"{exception.StackTrace}";
            }
            else
            {
                msg = $"{Environment.NewLine}[CRITICAL] {message}{Environment.NewLine}{Environment.NewLine}" +
                      $"{exceptionObj}{Environment.NewLine}";
            }
            Console.Error.WriteLine(msg);
        }

        public static void ShowCriticalInfo(string message, string title = null, bool suffixNewLine = true)
        {
            if (!string.IsNullOrEmpty(title)) Console.Title = title;
            if (suffixNewLine) Console.Error.WriteLine(message);
            else Console.Error.Write(message);
        }
    }
}