// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSettingsExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Chromely.Core.Helpers;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The cef settings extension.
    /// </summary>
    public static class CefSettingsExtension
    {
        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="cefSettings">
        /// The cef settings.
        /// </param>
        /// <param name="customSettings">
        /// The custom settings.
        /// </param>
        public static void Update(this CefSettings cefSettings, Dictionary<string, object> customSettings)
        {
            if (cefSettings == null || 
                customSettings == null ||
                customSettings.Count == 0)
            {
                return;
            }

            foreach (var setting in customSettings)
            {
                bool boolResult;
                int intResult;
                string strResult;

                switch (setting.Key)
                {
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
