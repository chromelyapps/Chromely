// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSettingKeys.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Helpers
{
    /// <summary>
    /// The cef setting keys.
    /// </summary>
    public static class CefSettingKeys
    {
        /// <summary>
        /// The single process.
        /// </summary>
        public const string SingleProcess = "SingleProcess";

        /// <summary>
        /// The no sandbox.
        /// </summary>
        public const string NoSandbox = "NoSandbox";

        /// <summary>
        /// The browser subprocess path.
        /// </summary>
        public const string BrowserSubprocessPath = "BrowserSubprocessPath";

        /// <summary>
        /// The multi threaded message loop.
        /// </summary>
        public const string MultiThreadedMessageLoop = "MultiThreadedMessageLoop";

        /// <summary>
        /// The external message pump.
        /// </summary>
        public const string ExternalMessagePump = "ExternalMessagePump";

        /// <summary>
        /// The windowless rendering enabled.
        /// </summary>
        public const string WindowlessRenderingEnabled = "WindowlessRenderingEnabled";

        /// <summary>
        /// The command line args disabled.
        /// </summary>
        public const string CommandLineArgsDisabled = "CommandLineArgsDisabled";

        /// <summary>
        /// The cache path.
        /// </summary>
        public const string CachePath = "CachePath";

        /// <summary>
        /// The user data path.
        /// </summary>
        public const string UserDataPath = "UserDataPath";

        /// <summary>
        /// The persist session cookies.
        /// </summary>
        public const string PersistSessionCookies = "PersistSessionCookies";

        /// <summary>
        /// The persist user preferences.
        /// </summary>
        public const string PersistUserPreferences = "PersistUserPreferences";

        /// <summary>
        /// The user agent.
        /// </summary>
        public const string UserAgent = "UserAgent";

        /// <summary>
        /// The product version.
        /// </summary>
        public const string ProductVersion = "ProductVersion";

        /// <summary>
        /// The locale.
        /// </summary>
        public const string Locale = "Locale";

        /// <summary>
        /// The log file.
        /// </summary>
        public const string LogFile = "LogFile";

        /// <summary>
        /// The log severity.
        /// </summary>
        public const string LogSeverity = "LogSeverity";

        /// <summary>
        /// The java script flags.
        /// </summary>
        public const string JavaScriptFlags = "JavaScriptFlags";

        /// <summary>
        /// The resources dir path.
        /// </summary>
        public const string ResourcesDirPath = "ResourcesDirPath";

        /// <summary>
        /// The locales dir path.
        /// </summary>
        public const string LocalesDirPath = "LocalesDirPath";

        /// <summary>
        /// The pack loading disabled.
        /// </summary>
        public const string PackLoadingDisabled = "PackLoadingDisabled";

        /// <summary>
        /// The remote debugging port.
        /// </summary>
        public const string RemoteDebuggingPort = "RemoteDebuggingPort";

        /// <summary>
        /// The uncaught exception stack size.
        /// </summary>
        public const string UncaughtExceptionStackSize = "UncaughtExceptionStackSize";

        /// <summary>
        /// The ignore certificate errors.
        /// </summary>
        public const string IgnoreCertificateErrors = "IgnoreCertificateErrors";

        /// <summary>
        /// The enable net security expiration.
        /// </summary>
        public const string EnableNetSecurityExpiration = "EnableNetSecurityExpiration";

        /// <summary>
        /// The accept language list.
        /// </summary>
        public const string AcceptLanguageList = "AcceptLanguageList";

        /// <summary>
        /// The focused node changed enabled.
        /// </summary>
        public const string FocusedNodeChangedEnabled = "FocusedNodeChangedEnabled";
    }
}
