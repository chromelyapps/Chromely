// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class JsObjectBinding
{
    public const string DEFAULTNAME = "boundAsync";
}

public static class StartUrlOption
{
    public const string ABSOLUTE = nameof(ABSOLUTE);
    public const string LOCALRESOURCE = nameof(LOCALRESOURCE);
    public const string ASSEMBLYRESOURCE = nameof(ASSEMBLYRESOURCE);
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
    public const string ASSEMBLYRESOURCE = nameof(ASSEMBLYRESOURCE);
    public const string LOCALREQUEST = nameof(LOCALREQUEST);
    public const string CUSTOM = nameof(CUSTOM);
    public const string EXTERNAL = nameof(EXTERNAL);
    public const string EXTERNALREQUEST = nameof(EXTERNALREQUEST);
}

public static class DefaultSchemeName
{
    public const string LOCALRESOURCE = "default-local-resource";
    public const string FOLDERRESOURCE = "default-folder-resource";
    public const string ASSEMBLYRESOURCE = "default-assembly-resource";
    public const string MIXASSEMBLYRESOURCE = "default-mix-assembly-resource";
    public const string LOCALREQUEST = "default-request-http";
    public const string GITHUBSITE = "chromely-site";
    public const string OWIN = "default-owin-http";
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
    public const string ENABLENETSECURITYEXPIRATION = nameof(ENABLENETSECURITYEXPIRATION);
    public const string ACCEPTLANGUAGELIST = nameof(ACCEPTLANGUAGELIST);
    public const string FOCUSEDNODECHANGEDENABLED = nameof(FOCUSEDNODECHANGEDENABLED);
    public const string FRAMEWORKDIRPATH = nameof(FRAMEWORKDIRPATH);
    public const string MAINBUNDLEPATH = nameof(MAINBUNDLEPATH);
}

public static class ResourceStatus
{
    public const string ZeroFileSize = nameof(ZeroFileSize);
    public const string FileNotFound = nameof(FileNotFound);
    public const string FileProcessingError = nameof(FileProcessingError);
}

public static class ResourceConstants
{
    public const HttpStatusCode StatusOK = HttpStatusCode.OK;
    public const string StatusOKText = "OK";
}

public static class RequestConstants
{
    public const string Referrer = "referrer";
}

public static class ResponseConstants
{
    public const int StatusOK = 200;
    public const string StatusOKText = "OK";
    public const int MinClientErrorStatusCode = 400;
    public const int MaxClientErrorStatusCode = 499;
    public const int MinServerErrorStatusCode = 500;
    public const string Header_AccessControlAllowOrigin = "Access-Control-Allow-Origin";
    public const string Header_CacheControl = "Cache-Control";
    public const string Header_AccessControlAllowMethods = "Access-Control-Allow-Methods";
    public const string Header_AccessControlAllowHeaders = "Access-Control-Allow-Headers";
    public const string Header_ContentType = "Content-Type";
    public const string Header_ContentTypeValue = "application/json";
}