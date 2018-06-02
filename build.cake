var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");

Task("Download.Cef")
    .Does(() => 
    {
        var x = "http://opensource.spotify.com/cefbuilds/cef_binary_3.3239.1723.g071d1c1_windows64_minimal.tar.bz2";
        var y = "http://opensource.spotify.com/cefbuilds/cef_binary_3.3202.1686.gd665578_windows64.tar.bz2";
        
        var cefVersion = XmlPeek("./src/CefGlue/Chromely.Unofficial.CefGlue.NetStd/Chromely.Unofficial.CefGlue.NetStd.csproj", "/Project/PropertyGroup/ProductVersion");
        Information(cefVersion);
    });

Task("Build")
    .IsDependentOn("Download.Cef")
    .Does(() =>
    {
        Information("Hello world");
    });


Task("Default").IsDependentOn("Build");
RunTarget(target);