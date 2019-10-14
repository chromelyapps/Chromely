// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefSettingKeys.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
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
        public const string SingleProcess = nameof(SingleProcess);

        /// <summary>
        /// The no sandbox.
        /// </summary>
        public const string NoSandbox = nameof(NoSandbox);

        /// <summary>
        /// The browser subprocess path.
        /// </summary>
        public const string BrowserSubprocessPath = nameof(BrowserSubprocessPath);

        /// <summary>
        /// The multi threaded message loop.
        /// </summary>
        public const string MultiThreadedMessageLoop = nameof(MultiThreadedMessageLoop);

        /// <summary>
        /// The external message pump.
        /// </summary>
        public const string ExternalMessagePump = nameof(ExternalMessagePump);

        /// <summary>
        /// The windowless rendering enabled.
        /// </summary>
        public const string WindowlessRenderingEnabled = nameof(WindowlessRenderingEnabled);

        /// <summary>
        /// The command line args disabled.
        /// </summary>
        public const string CommandLineArgsDisabled = nameof(CommandLineArgsDisabled);

        /// <summary>
        /// The cache path.
        /// </summary>
        public const string CachePath = nameof(CachePath);

        /// <summary>
        /// The user data path.
        /// </summary>
        public const string UserDataPath = nameof(UserDataPath);

        /// <summary>
        /// The persist session cookies.
        /// </summary>
        public const string PersistSessionCookies = nameof(PersistSessionCookies);

        /// <summary>
        /// The persist user preferences.
        /// </summary>
        public const string PersistUserPreferences = nameof(PersistUserPreferences);

        /// <summary>
        /// The user agent.
        /// </summary>
        public const string UserAgent = nameof(UserAgent);

        /// <summary>
        /// The product version.
        /// </summary>
        public const string ProductVersion = nameof(ProductVersion);

        /// <summary>
        /// The locale.
        /// </summary>
        public const string Locale = nameof(Locale);

        /// <summary>
        /// The log file.
        /// </summary>
        public const string LogFile = nameof(LogFile);

        /// <summary>
        /// The log severity.
        /// </summary>
        public const string LogSeverity = nameof(LogSeverity);

        /// <summary>
        /// The java script flags.
        /// </summary>
        public const string JavaScriptFlags = nameof(JavaScriptFlags);

        /// <summary>
        /// The resources dir path.
        /// </summary>
        public const string ResourcesDirPath = nameof(ResourcesDirPath);

        /// <summary>
        /// The locales dir path.
        /// </summary>
        public const string LocalesDirPath = nameof(LocalesDirPath);

        /// <summary>
        /// The pack loading disabled.
        /// </summary>
        public const string PackLoadingDisabled = nameof(PackLoadingDisabled);

        /// <summary>
        /// The remote debugging port.
        /// </summary>
        public const string RemoteDebuggingPort = nameof(RemoteDebuggingPort);

        /// <summary>
        /// The uncaught exception stack size.
        /// </summary>
        public const string UncaughtExceptionStackSize = nameof(UncaughtExceptionStackSize);

        /// <summary>
        /// The ignore certificate errors.
        /// </summary>
        public const string IgnoreCertificateErrors = nameof(IgnoreCertificateErrors);

        /// <summary>
        /// The enable net security expiration.
        /// </summary>
        public const string EnableNetSecurityExpiration = nameof(EnableNetSecurityExpiration);

        /// <summary>
        /// The accept language list.
        /// </summary>
        public const string AcceptLanguageList = nameof(AcceptLanguageList);

        /// <summary>
        /// The focused node changed enabled.
        /// </summary>
        public const string FocusedNodeChangedEnabled = nameof(FocusedNodeChangedEnabled);
    }
}
