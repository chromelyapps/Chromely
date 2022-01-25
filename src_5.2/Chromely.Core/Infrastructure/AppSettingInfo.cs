// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

/// <summary>
/// Utility class for application settings.
/// </summary>
public static class AppSettingInfo
{
    /// <summary>
    /// Gets the application setting file path.
    /// </summary>
    /// <param name="platform">The OS platform type.</param>
    /// <param name="appName">The application name.</param>
    /// <param name="onSave">The flag to indicate saving of application settings.</param>
    /// <returns>The application setting file path.</returns>
    public static string? GetSettingsFilePath(ChromelyPlatform platform, string appName = "chromely", bool onSave = false)
    {
        try
        {
            var appSettingsDir = string.Empty;
            var fileName = $"{appName}_appsettings.config";

            switch (platform)
            {
                case ChromelyPlatform.Windows:
                    appSettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "chromely");
                    break;

                case ChromelyPlatform.Linux:
                    appSettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "chromely");
                    break;

                case ChromelyPlatform.MacOSX:
                    appSettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.DoNotVerify);
                    appSettingsDir = appSettingsDir.Replace("/Documents", "/Library/Application Support/chromely/");
                    break;
            }

            if (onSave)
            {
                Directory.CreateDirectory(appSettingsDir);
                if (Directory.Exists(appSettingsDir))
                {
                    return Path.Combine(appSettingsDir, fileName);
                }
            }
            else
            {
                return Path.Combine(appSettingsDir, fileName);
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return default;
    }
}