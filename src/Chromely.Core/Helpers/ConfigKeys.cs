namespace Chromely.Core.Helpers
{
    public static class StartUrlOption
    {
        public const string ABSOLUTE = nameof(ABSOLUTE);
        public const string LOCALRESOURCE = nameof(LOCALRESOURCE);
        public const string FILEPROTOCOL = nameof(FILEPROTOCOL);
    }

    public static class WindowStateOption
    {
        public const string NORMAL = nameof(NORMAL);
        public const string MAXIMIZE = nameof(MAXIMIZE);
        public const string FULLSCREEN = nameof(FULLSCREEN);
    }

    public static class UrlSchemeOption
    {
        public const string NONE = nameof(NONE);
        public const string RESOURCE = nameof(RESOURCE);
        public const string COMMAND = nameof(COMMAND);
        public const string CUSTOM = nameof(CUSTOM);
        public const string EXTERNAL = nameof(EXTERNAL);
    }

    public static class LogSeverityOption
    {
        public const string DEFAULT = nameof(DEFAULT);
        public const string VERBOSE = nameof(VERBOSE);
        public const string INFO = nameof(INFO);
        public const string ERROR = nameof(ERROR);
        public const string EXTERNAL = nameof(EXTERNAL);
        public const string FATAL = nameof(FATAL);
        public const string DISABLE = nameof(DISABLE);
    }

    public static class CefSettingKeys
    {
        public const string SINGLEPROCESS = nameof(SINGLEPROCESS);
        public const string NOSANDBOX = nameof(NOSANDBOX);
        public const string BROWSERSUBPROCESSPATH = nameof(BROWSERSUBPROCESSPATH);
        public const string MULTITHREADEDMESSAGELOOP = nameof(MULTITHREADEDMESSAGELOOP);
        public const string EXTERNALMESSAGEPUMP = nameof(EXTERNALMESSAGEPUMP);
        public const string WINDOWLESSRENDERINGENABLED = nameof(WINDOWLESSRENDERINGENABLED);
        public const string COMMANDLINEARGSDISABLED = nameof(COMMANDLINEARGSDISABLED);
        public const string CACHEPATH = nameof(CACHEPATH);
        public const string USERDATAPATH = nameof(USERDATAPATH);
        public const string PERSISTSESSIONCOOKIES = nameof(PERSISTSESSIONCOOKIES);
        public const string PERSISTUSERPREFERENCES = nameof(PERSISTUSERPREFERENCES);
        public const string USERAGENT = nameof(USERAGENT);
        public const string PRODUCTVERSION = nameof(PRODUCTVERSION);
        public const string LOCALE = nameof(LOCALE);
        public const string CEFLOGFILE = nameof(CEFLOGFILE);
        public const string LOGFILE = nameof(LOGFILE);
        public const string LOGSEVERITY = nameof(LOGSEVERITY);
        public const string JAVASCRIPTFLAGS = nameof(JAVASCRIPTFLAGS);
        public const string RESOURCESDIRPATH = nameof(RESOURCESDIRPATH);
        public const string LOCALESDIRPATH = nameof(LOCALESDIRPATH);
        public const string PACKLOADINGDISABLED = nameof(PACKLOADINGDISABLED);
        public const string REMOTEDEBUGGINGPORT = nameof(REMOTEDEBUGGINGPORT);
        public const string UNCAUGHTEXCEPTIONSTACKSIZE = nameof(UNCAUGHTEXCEPTIONSTACKSIZE);
        public const string IGNORECERTIFICATEERRORS = nameof(IGNORECERTIFICATEERRORS);
        public const string ENABLENETSECURITYEXPIRATION = nameof(ENABLENETSECURITYEXPIRATION);
        public const string ACCEPTLANGUAGELIST = nameof(ACCEPTLANGUAGELIST);
        public const string FOCUSEDNODECHANGEDENABLED = nameof(FOCUSEDNODECHANGEDENABLED);
    }
}
