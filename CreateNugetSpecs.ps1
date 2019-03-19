
Param(
    [string]$TemplatesPath = "Chromely-Win\Artifacts"
)

Get-ChildItem $TemplatesPath -Filter *.nuspec | 
Foreach-Object {

    Write-Output $_.FullName

    $content = Get-Content -Path $_.FullName
    $content = $content -Replace '{version}', $packageVersion -Replace '{releaseNotes}', $releaseNotes
    Set-Content -Path $_.Name -Value $content

}

