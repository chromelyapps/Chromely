if not exist tools mkdir tools
if not exist tools\nuget.exe powershell -Command "Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile tools\nuget.exe" & cd tools & nuget.exe install -ExcludeVersion & cd ..

tools\cake\cake.exe build.cake -target=%*