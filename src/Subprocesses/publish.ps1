Function Set-VSEnvironment() {
    # Just use whatever is VS's official .bat to setup command prompt
    if (($env:FrameworkDir -eq $null) -or 
        ($env:FrameworkDir -eq "")) {

        # A few places, in decreasing priority, for script that setups up VS environment
        $VsDevCmdPaths = @(
            "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat",
            "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat", 
            "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat")
        foreach ($VsDevCmdPath in $VsDevCmdPaths) {
            if (Test-Path $VsDevCmdPath) {
                cmd /c """$VsDevCmdPath""&set" |
                    foreach {
                        # Just dumping env vars to console/log
                        if ($_ -match "=") {
                            $v = $_.split("="); 
                            Write-Host "Setting $($v[0])" -ForegroundColor Yellow
                            Set-Item -force -path "ENV:\$($v[0])"  -value "$($v[1])"
                        }
                    }
                write-host "`nVisual Studio Command Prompt variables set via $VsDevCmdPath" -ForegroundColor Yellow
                break;
            }
        }
    } else {
        Write-Host "Visual Studio environment already setup" -ForegroundColor Yellow
    }
}

# Just setup the VS CLI environment
Set-VSEnvironment

# Build version
Write-Host "msbuild-version:" msbuild /version

.\nuget.exe restore ..\ChromelySolution.sln

# Win Builds 
msbuild .\Chromely.CefGlue.Win.Subprocess\Chromely.CefGlue.Win.Subprocess.csproj /p:Configuration=Release /p:Platform=x86 /p:AllowUnsafeBlocks=true /p:OutputPath="..\subprocess_binaries\win32"
msbuild .\Chromely.CefGlue.Win.Subprocess\Chromely.CefGlue.Win.Subprocess.csproj /p:Configuration=Release /p:Platform=x64 /p:AllowUnsafeBlocks=true /p:OutputPath="..\subprocess_binaries\win64"


# Linux Build 
dotnet restore ".\Chromely.CefGlue.Linux.Subprocess\Chromely.CefGlue.Linux.Subprocess.csproj"
dotnet publish -c release -r ubuntu.16.10-x64 --self-contained false --output "subprocess_binaries\linux" ".\Chromely.CefGlue.Linux.Subprocess\Chromely.CefGlue.Linux.Subprocess.csproj"

# Mac Builds 
dotnet restore ".\Chromely.CefGlue.Mac.Subprocess\Chromely.CefGlue.Mac.Subprocess.csproj"
dotnet publish -c release -r osx.10.12-x64 --self-contained false  --output "subprocess_binaries\mac" ".\Chromely.CefGlue.Mac.Subprocess\Chromely.CefGlue.Mac.Subprocess.csproj"