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

namespace Chromely.CefGlue.Gtk
{
    using System.Collections.Generic;
    using Chromely.Core.Helpers;
    using Xilium.CefGlue;

    public static class CefSettingsExtension
    {
        public static void Update(this CefSettings cefSettings, Dictionary<string, object> customSettings)
        {
            if ((cefSettings == null) || (customSettings == null))
            {
                return;
            }

            if (customSettings.Count == 0)
            {
                return;
            }

            foreach (var setting in customSettings)
            {
                bool boolResult = false;
                int intResult = 0;
                string strResult = string.Empty;

                switch (setting.Key)
                {
                    case CefSettingKeys.SingleProcess:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.SingleProcess = boolResult;
                        }
                        break;

                    case CefSettingKeys.NoSandbox:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.NoSandbox = boolResult;
                        }
                        break;

                    case CefSettingKeys.BrowserSubprocessPath:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.BrowserSubprocessPath = strResult;
                        }
                        break;

                    case CefSettingKeys.MultiThreadedMessageLoop:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.MultiThreadedMessageLoop = boolResult;
                        }
                        break;

                    case CefSettingKeys.ExternalMessagePump:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.ExternalMessagePump = boolResult;
                        }
                        break;

                    case CefSettingKeys.WindowlessRenderingEnabled:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.WindowlessRenderingEnabled = boolResult;
                        }
                        break;

                    case CefSettingKeys.CommandLineArgsDisabled:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.CommandLineArgsDisabled = boolResult;
                        }
                        break;

                    case CefSettingKeys.CachePath:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.CachePath = strResult;
                        }
                        break;

                    case CefSettingKeys.UserDataPath:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.UserDataPath = strResult;
                        }
                        break;

                    case CefSettingKeys.PersistSessionCookies:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.PersistSessionCookies = boolResult;
                        }
                        break;

                    case CefSettingKeys.PersistUserPreferences:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.PersistUserPreferences = boolResult;
                        }
                        break;

                    case CefSettingKeys.UserAgent:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.UserAgent = strResult;
                        }
                        break;

                    case CefSettingKeys.ProductVersion:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.ProductVersion = strResult;
                        }
                        break;

                    case CefSettingKeys.Locale:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.Locale = strResult;
                        }
                        break;

                    case CefSettingKeys.LogFile:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.LogFile = strResult;
                        }
                        break;

                    case CefSettingKeys.LogSeverity:
                        if (setting.Value.TryParseInteger(out intResult))
                        {
                            cefSettings.LogSeverity = (CefLogSeverity)intResult;
                        }
                        break;

                    case CefSettingKeys.JavaScriptFlags:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.JavaScriptFlags = strResult;
                        }
                        break;

                    case CefSettingKeys.ResourcesDirPath:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.ResourcesDirPath = strResult;
                        }
                        break;

                    case CefSettingKeys.LocalesDirPath:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.LocalesDirPath = strResult;
                        }
                        break;

                    case CefSettingKeys.PackLoadingDisabled:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.PackLoadingDisabled = boolResult;
                        }
                        break;

                    case CefSettingKeys.RemoteDebuggingPort:
                        if (setting.Value.TryParseInteger(out intResult))
                        {
                            cefSettings.RemoteDebuggingPort = intResult;
                        }
                        break;

                    case CefSettingKeys.UncaughtExceptionStackSize:
                        if (setting.Value.TryParseInteger(out intResult))
                        {
                            cefSettings.UncaughtExceptionStackSize = intResult;
                        }
                        break;

                    case CefSettingKeys.IgnoreCertificateErrors:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.IgnoreCertificateErrors = boolResult;
                        }
                        break;

                    case CefSettingKeys.EnableNetSecurityExpiration:
                        if (setting.Value.TryParseBoolean(out boolResult))
                        {
                            cefSettings.EnableNetSecurityExpiration = boolResult;
                        }
                        break;

                    case CefSettingKeys.AcceptLanguageList:
                        if (setting.Value.TryParseString(out strResult))
                        {
                            cefSettings.AcceptLanguageList = strResult;
                        }
                        break;

                    // Not supported by CefGlue
                    case CefSettingKeys.FocusedNodeChangedEnabled:
                        break;
                }
            }
        }
    }
}
