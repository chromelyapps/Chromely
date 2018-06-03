#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression
#addin nuget:?package=Cake.SemVer
#addin nuget:?package=semver&version=2.0.4

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
var debugPack = Argument("debugpack", false);
var cefMap = new Dictionary<string, string>()
{
    ["64.0.0.0"] = "3.3282.1741.gcd94615",
    ["65.0.0.0"] = "3.3325.1751.ge5b78a5"
};

void RestoreCefBinary(string packageName ,string baseUrl = "http://opensource.spotify.com/cefbuilds/", string targetDirectory = "./dist/redist")
{
    CreateDirectory(targetDirectory);

    var downloadUri = $"{baseUrl}{packageName}";
    var targetFileName = $"{targetDirectory}/{packageName}";

    if(FileExists(targetFileName))
    {
        Information($"[Skip Downloading]: {downloadUri} to {targetFileName} cause it already exists");
    }
    else
    {
        Information($"[Downloading]: {downloadUri} to {targetFileName}");
        DownloadFile(downloadUri, targetFileName);
    }

    var targetExtractDirectory = $"{targetDirectory}/{System.IO.Path.GetFileNameWithoutExtension(packageName)}";

    
    if(DirectoryExists(targetExtractDirectory))
    {
        Information($"[Skip Extracting]: {targetFileName} to {targetExtractDirectory} cause it already exists");
    }
    else
    {
        Information($"[Extracting]: {targetFileName} to {targetExtractDirectory}");
        BZip2Uncompress(targetFileName, targetExtractDirectory);
    }
    
    var platform = packageName.Split('_')[3];
    var platformDirectory = $"{targetDirectory}/{platform}";
    CreateDirectory(platformDirectory);

    var targetExtractDirectory2 = $"{targetDirectory}/{System.IO.Path.GetFileNameWithoutExtension(packageName)}/{System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileNameWithoutExtension(packageName))}";
    var resourceDirectory = $"{targetExtractDirectory2}/Resources";
    var releaseDirectory = $"{targetExtractDirectory2}/Release";

    if(GetFiles($"{platformDirectory}/*.*").Count > 0)
    {
        Information($"[Skip Copy platform]: {releaseDirectory} to {platformDirectory} cause there are already files");
        Information($"[Skip Copy platform]: {resourceDirectory} to {platformDirectory} cause there are already files");
    }
    else
    {
        if(DirectoryExists(releaseDirectory))
        {
            Information($"[Copy release]: {releaseDirectory} to {platformDirectory}");
            CopyDirectory(releaseDirectory, platformDirectory);
        }
        if(DirectoryExists(resourceDirectory))
        {
            Information($"[Copy resources]: {resourceDirectory} to {platformDirectory}");       
            CopyDirectory(resourceDirectory, platformDirectory);
        }  
    }
}

void RestoreCef(bool force = false)
{
    var cefWinVersion = XmlPeek("./src/Chromely.CefGlue.Winapi/Chromely.CefGlue.Winapi.csproj", "/Project/PropertyGroup/Version");
    var cefVersion = cefMap[cefWinVersion];

    var win32Uri = $"cef_binary_{cefVersion}_windows32_minimal.tar.bz2";
    var win64Uri = $"cef_binary_{cefVersion}_windows64_minimal.tar.bz2";
    var linux32Uri = $"cef_binary_{cefVersion}_linux32_minimal.tar.bz2";
    var linux64Uri = $"cef_binary_{cefVersion}_linux64_minimal.tar.bz2";
    var mac64Uri = $"cef_binary_{cefVersion}_macosx64_minimal.tar.bz2";
    
    if(force || IsRunningOnWindows())
    {
        RestoreCefBinary(win32Uri);
        RestoreCefBinary(win64Uri);
    }

    if(force || IsRunningOnUnix())
    {
        RestoreCefBinary(linux32Uri);
        RestoreCefBinary(linux64Uri);
        RestoreCefBinary(mac64Uri);
    }
}

string GetPackVersion()
{
    var cefWinVersion = System.Version.Parse(XmlPeek("./src/Chromely.CefGlue.Winapi/Chromely.CefGlue.Winapi.csproj", "/Project/PropertyGroup/Version"));

    if(debugPack)
    {
        var debugPackFileName = "./debugpack.txt";
        if(!FileExists(debugPackFileName))
        {
            System.IO.File.WriteAllText(debugPackFileName, "1");
        }

        var debugPackCounter = int.Parse(System.IO.File.ReadAllText(debugPackFileName));    
        var semver = CreateSemVer(cefWinVersion.Major, cefWinVersion.Minor, debugPackCounter);    
        System.IO.File.WriteAllText(debugPackFileName, (debugPackCounter + 1).ToString());
        return semver.ToString();
    }

    return CreateSemVer(cefWinVersion.Major, cefWinVersion.Minor, cefWinVersion.Build).ToString();
}

void Pack(bool force = false, string targetDirectory = "./dist/nuget")
{
    var semver = GetPackVersion();

    Information(semver);

    var settings = new NuGetPackSettings()
    {
        Version = semver,
        OutputDirectory = targetDirectory,
    };
    
    NuGetPack("./src/Chromely.Cef.Redist/Chromely.Cef.Redist.nuspec", settings);    
    if(force || IsRunningOnWindows())
    {
        NuGetPack("./src/Chromely.Cef.Redist/Windows/Chromely.Cef.Redist.Windows.nuspec", settings);
        NuGetPack("./src/Chromely.Cef.Redist/win-64/Chromely.Cef.Redist.win-64.nuspec", settings);
        NuGetPack("./src/Chromely.Cef.Redist/win-86/Chromely.Cef.Redist.win-86.nuspec", settings);
    }
    if(force || IsRunningOnUnix())
    {
        NuGetPack("./src/Chromely.Cef.Redist/Linux/Chromely.Cef.Redist.Linux.nuspec", settings);
        NuGetPack("./src/Chromely.Cef.Redist/linux-86/Chromely.Cef.Redist.linux-86.nuspec", settings);
        NuGetPack("./src/Chromely.Cef.Redist/linux-64/Chromely.Cef.Redist.linux-64.nuspec", settings);
    }
}

Task("Clean.Force")
    .IsDependentOn("Clean")
    .Does(() => CleanDirectory("./dist"));

Task("Clean")
    .Does(() =>
    {
        CleanDirectory( "./dist/test");
        DotNetCoreClean("./src/ChromelySolution.sln");
    });

Task("Restore.Cef.Force")
    .Does(() => RestoreCef(force: true));

Task("Restore.Cef")
    .Does(() => RestoreCef());

Task("Restore")
    .IsDependentOn("Restore.Cef")
    .Does(() => DotNetCoreRestore("./src/ChromelySolution.sln"));

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => DotNetCoreBuild("./src/ChromelySolution.sln", new DotNetCoreBuildSettings
    {
        NoRestore = true
    }));

Task("Pack.Redist")
    .IsDependentOn("Restore.Cef")
    .Does(() => Pack());

Task("Pack.Redist.Force")
    .IsDependentOn("Restore.Cef.Force")
    .Does(() => Pack(true));

Task("Build.Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Build.Force")
    .IsDependentOn("Clean.Force")
    .IsDependentOn("Build");

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
    {
        var settings = new DotNetCoreTestSettings
        {
            ResultsDirectory = "./dist/test",
            NoBuild = true,
            NoRestore = true,
        };

        var projectFiles = GetFiles("./src/tests/**/*.csproj");
        foreach(var file in projectFiles)
        {
            DotNetCoreTest(file.FullPath, settings);
        }
    });

Task("ReBuild").IsDependentOn("Build.Rebuild"); // Alias for rebuld

Task("Default")
    .IsDependentOn("Build");
RunTarget(target);