
Param(
    [string]$ReleaseNotesFileName = "ReleaseNotes.md"
)

Write-Output "Get Target Package Version"
$semVer = "\* (?<semVer>\d+\.\d+\.\d+(\.\d+)?) +(?<relNotes>.*)"
$lines = Get-Content $ReleaseNotesFileName
$version = $lines | Select-String -Pattern $semVer | Select-Object -First 1
$version -match $semVer
$packageVersion = $Matches.semVer
$releaseNotes = $Matches.relNotes
Write-Output "The current version is: $packageVersion"
Write-Output "Release notes: $releaseNotes"
Write-Host "##vso[task.setvariable variable=PACKAGE_VERSION;]$packageVersion"
Write-Output ""
