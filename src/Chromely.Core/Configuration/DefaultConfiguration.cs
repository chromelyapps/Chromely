// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Configuration;

/// <summary>
/// Implements default values for <see cref="IChromelyConfiguration"/>.
/// </summary>
public class DefaultConfiguration : IChromelyConfiguration
{
    /// <summary>
    /// Initializes a new instance of <see cref="DefaultConfiguration"/>.
    /// </summary>
    public DefaultConfiguration()
    {
        AppName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Chromely App";
        Platform = ChromelyRuntime.Platform;
        AppExeLocation = AppDomain.CurrentDomain.BaseDirectory;
        StartUrl = "local://app/index.html";
        DebuggingMode = true;
        UrlSchemes = new List<UrlScheme>();
        CefDownloadOptions = new CefDownloadOptions();
        WindowOptions = new WindowOptions();
        if (string.IsNullOrWhiteSpace(WindowOptions.Title))
        {
            WindowOptions.Title = AppName;
        }

        // These are all default schemes.
        // They can be removed or replaced.
        UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme(DefaultSchemeName.LOCALRESOURCE, "local", string.Empty, string.Empty, UrlSchemeType.LocalResource),
                new UrlScheme(DefaultSchemeName.LOCALREQUEST, "http", "chromely.com", string.Empty, UrlSchemeType.LocalRequest),
                new UrlScheme(DefaultSchemeName.OWIN, "http", "chromely.owin.com", string.Empty, UrlSchemeType.Owin),
                new UrlScheme(DefaultSchemeName.GITHUBSITE, string.Empty, string.Empty, "https://github.com/chromelyapps/Chromely", UrlSchemeType.ExternalBrowser, true)
            });

        CustomSettings = new Dictionary<string, string>()
        {
            ["cefLogFile"] = "logs\\chromely.cef.log",
            ["logSeverity"] = "info",
            ["locale"] = "en-US"
        };
    }

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public string? AppName { get; set; }

    /// <summary>
    /// Gets or sets the start URL.
    /// </summary>
    public string StartUrl { get; set; }

    /// <summary>
    /// Gets or sets the application executable location.
    /// </summary>
    public string AppExeLocation { get; set; }

    /// <summary>
    /// Gets or sets the Chromely version.
    /// </summary>
    public string? ChromelyVersion { get; set; }

    /// <summary>
    /// Gets or sets the platform.
    /// </summary>
    public ChromelyPlatform Platform { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether debugging is enabled or not.
    /// </summary>
    public bool DebuggingMode { get; set; }

    /// <summary>
    /// Gets or sets the dev tools URL.
    /// </summary>
    public string? DevToolsUrl { get; set; }

    /// <summary>
    /// Gets or sets the command line arguments.
    /// </summary>
    public Dictionary<string, string>? CommandLineArgs { get; set; }

    /// <summary>
    /// Gets or sets the command line options.
    /// </summary>
    public List<string>? CommandLineOptions { get; set; }

    /// <summary>
    /// Gets or sets the custom settings.
    /// </summary>
    public Dictionary<string, string>? CustomSettings { get; set; }

    /// <summary>
    /// Gets or sets the extension data.
    /// </summary>
    public Dictionary<string, object>? ExtensionData { get; set; }

    /// <summary>
    /// Gets or sets the java script executor.
    /// </summary>
    public IChromelyJavaScriptExecutor? JavaScriptExecutor { get; set; }

    /// <summary>
    /// Gets or sets the URL schemes.
    /// </summary>
    public List<UrlScheme> UrlSchemes { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Configuration.CefDownloadOptions"/> for CEF download options.
    /// </summary>
    public CefDownloadOptions CefDownloadOptions { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IWindowOptions"/> for window options.
    /// </summary>
    public IWindowOptions WindowOptions { get; set; }

    /// <summary>
    /// Create configuration instance for the OS platform app is running on.
    /// </summary>
    /// <returns>Instance of <see cref="IChromelyConfiguration"/>.</returns>
    public static IChromelyConfiguration CreateForRuntimePlatform()
    {
        return CreateForPlatform(ChromelyRuntime.Platform);
    }

    /// <summary>
    /// Create configuration instance for OS platform specifying the platform.
    /// </summary>
    /// <param name="platform">Specifying the OS platfor to create <see cref="IChromelyConfiguration"/> instance for.</param>
    /// <returns>Instance of <see cref="IChromelyConfiguration"/>.</returns>
    public static IChromelyConfiguration CreateForPlatform(ChromelyPlatform platform)
    {
        IChromelyConfiguration config = new DefaultConfiguration();

        try
        {
            switch (platform)
            {
                case ChromelyPlatform.Windows:
                    config.WindowOptions.CustomStyle = new WindowCustomStyle(0, 0);
                    config.WindowOptions.UseCustomStyle = false;
                    break;

                case ChromelyPlatform.Linux:
                    config.CommandLineArgs = new Dictionary<string, string>
                    {
                        ["disable-gpu"] = "1"
                    };

                    config.CommandLineOptions = new List<string>()
                        {
                            "no-zygote",
                            "disable-gpu"
                        };
                    break;

                case ChromelyPlatform.MacOSX:
                    break;
            }

            return config;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return config;
    }
}