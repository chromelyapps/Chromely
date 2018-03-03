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

namespace Chromely.Core
{
    public static class CefSettingKeys
    {
        public const string SingleProcess = "SingleProcess";
        public const string NoSandbox = "NoSandbox";
        public const string BrowserSubprocessPath = "BrowserSubprocessPath";
        public const string MultiThreadedMessageLoop = "MultiThreadedMessageLoop";
        public const string ExternalMessagePump = "ExternalMessagePump";
        public const string WindowlessRenderingEnabled = "WindowlessRenderingEnabled";
        public const string CommandLineArgsDisabled = "CommandLineArgsDisabled";
        public const string CachePath = "CachePath";
        public const string UserDataPath = "UserDataPath";
        public const string PersistSessionCookies = "PersistSessionCookies";
        public const string PersistUserPreferences = "PersistUserPreferences";
        public const string UserAgent = "UserAgent";
        public const string ProductVersion = "ProductVersion";
        public const string Locale = "Locale";
        public const string LogFile = "LogFile";
        public const string LogSeverity = "LogSeverity";
        public const string JavaScriptFlags = "JavaScriptFlags";
        public const string ResourcesDirPath = "ResourcesDirPath";
        public const string LocalesDirPath = "LocalesDirPath";
        public const string PackLoadingDisabled = "PackLoadingDisabled";
        public const string RemoteDebuggingPort = "RemoteDebuggingPort";
        public const string UncaughtExceptionStackSize = "UncaughtExceptionStackSize";
        public const string IgnoreCertificateErrors = "IgnoreCertificateErrors";
        public const string EnableNetSecurityExpiration = "EnableNetSecurityExpiration";
        public const string AcceptLanguageList = "AcceptLanguageList";
        public const string FocusedNodeChangedEnabled = "FocusedNodeChangedEnabled";
    }
}
