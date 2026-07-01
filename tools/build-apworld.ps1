# build-apworld.ps1
# Packages the davethediver apworld into a .apworld file (zip archive)
# Run from the repo root: .\tools\build-apworld.ps1
# Run with -Install to also copy it into Archipelago's worlds folder

param(
    [switch]$Install
)

$ErrorActionPreference = "Stop"

$repoRoot    = Split-Path $PSScriptRoot -Parent
$sourceDir   = Join-Path $repoRoot "apworld\davethediver"
$outputFile  = Join-Path $repoRoot "davethediver.apworld"
$installDir  = "C:\ProgramData\Archipelago\custom_worlds"

if (-not (Test-Path $sourceDir)) {
    Write-Error "Source directory not found: $sourceDir"
    exit 1
}

# Remove old build if present
if (Test-Path $outputFile) {
    Remove-Item $outputFile -Force
    Write-Host "Removed old davethediver.apworld"
}

# Package: zip the contents of the davethediver folder (not the folder itself)
# Archipelago expects the apworld zip to contain the world files at the root level
# Compress-Archive only supports .zip, so we zip first then rename to .apworld
$tempZip = "$outputFile.zip"
if (Test-Path $tempZip) { Remove-Item $tempZip -Force }
Compress-Archive -Path "$sourceDir\*" -DestinationPath $tempZip
Move-Item $tempZip $outputFile

Write-Host ""
Write-Host "✅ Built: $outputFile" -ForegroundColor Green

if ($Install) {
    if (-not (Test-Path $installDir)) {
        Write-Error "Archipelago worlds folder not found: $installDir`nIs Archipelago installed?"
        exit 1
    }
    $dest = Join-Path $installDir "davethediver.apworld"
    Copy-Item $outputFile $dest -Force
    Write-Host "✅ Installed to: $dest" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "To install, copy davethediver.apworld to:"
    Write-Host "  $installDir"
    Write-Host ""
    Write-Host "Or run with -Install to copy automatically:"
    Write-Host "  .\tools\build-apworld.ps1 -Install"
}
