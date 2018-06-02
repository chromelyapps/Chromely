#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");

var cefMap = new Dictionary<string, string>()
{
    ["64.0.0.0"] = "3.3282.1741.gcd94615",
    ["65.0.0.0"] = "3.3325.1751.ge5b78a5"
};

void RestoreCefBinary(string packageName ,string baseUrl = "http://opensource.spotify.com/cefbuilds/", string targetDirectory = "./dist")
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

Task("Clean.Force")
    .IsDependentOn("Clean")
    .Does(() => CleanDirectory("./dist"));

Task("Clean")
    .Does(() => DotNetCoreClean("./src/ChromelySolution.sln"));

Task("Restore.Cef")
    .Does(() => 
    {
        var cefWinVersion = XmlPeek("./src/Chromely.CefGlue.Winapi/Chromely.CefGlue.Winapi.csproj", "/Project/PropertyGroup/Version");
        var cefVersion = cefMap[cefWinVersion];

        var win32Uri = $"cef_binary_{cefVersion}_windows32_minimal.tar.bz2";
        var win64Uri = $"cef_binary_{cefVersion}_windows64_minimal.tar.bz2";
        var linux32Uri = $"cef_binary_{cefVersion}_linux32_minimal.tar.bz2";
        var linux64Uri = $"cef_binary_{cefVersion}_linux64_minimal.tar.bz2";
        var mac64Uri = $"cef_binary_{cefVersion}_macosx64_minimal.tar.bz2";
        
        RestoreCefBinary(win32Uri);
        RestoreCefBinary(win64Uri);
        RestoreCefBinary(linux32Uri);
        RestoreCefBinary(linux64Uri);
        RestoreCefBinary(mac64Uri);     
    });

Task("Restore")
    .IsDependentOn("Restore.Cef")
    .Does(() => DotNetCoreRestore("./src/ChromelySolution.sln"));


Task("Build")
    .IsDependentOn("Restore")
    .Does(() => DotNetCoreBuild("./src/ChromelySolution.sln"));

Task("Build.Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Build.Force")
    .IsDependentOn("Clean.Force")
    .IsDependentOn("Build");

Task("Default")
    .IsDependentOn("Build");
RunTarget(target);