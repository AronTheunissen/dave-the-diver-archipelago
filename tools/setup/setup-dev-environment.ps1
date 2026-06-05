# Dave the Diver Archipelago - Development Environment Setup Script
# This script automates the setup of your development environment

param(
    [string]$GamePath = "",
    [switch]$SkipDotNet = $false,
    [switch]$SkipArchipelago = $false
)

Write-Host "=== Dave the Diver Archipelago - Development Environment Setup ===" -ForegroundColor Cyan
Write-Host ""

# Check Python
Write-Host "Checking Python installation..." -ForegroundColor Yellow
try {
    $pythonVersion = python --version 2>&1
    Write-Host "✓ Python found: $pythonVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ Python not found. Please install Python 3.10+ from python.org" -ForegroundColor Red
    exit 1
}

# Check Git
Write-Host "Checking Git installation..." -ForegroundColor Yellow
try {
    $gitVersion = git --version 2>&1
    Write-Host "✓ Git found: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ Git not found. Please install Git from git-scm.com" -ForegroundColor Red
    exit 1
}

# Check .NET SDK
if (-not $SkipDotNet) {
    Write-Host "Checking .NET SDK installation..." -ForegroundColor Yellow
    try {
        $dotnetVersion = dotnet --version 2>&1
        Write-Host "✓ .NET SDK found: $dotnetVersion" -ForegroundColor Green
    } catch {
        Write-Host "✗ .NET SDK not found." -ForegroundColor Red
        Write-Host "  Downloading .NET SDK 8.0..." -ForegroundColor Yellow
        
        $dotnetInstallerUrl = "https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-8.0.404-windows-x64-installer"
        Write-Host "  Please download and install .NET SDK from:" -ForegroundColor Yellow
        Write-Host "  https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "  After installation, re-run this script." -ForegroundColor Yellow
        
        $openBrowser = Read-Host "Open download page in browser? (Y/n)"
        if ($openBrowser -ne 'n') {
            Start-Process "https://dotnet.microsoft.com/download/dotnet/8.0"
        }
        exit 1
    }
}

# Navigate to project root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent (Split-Path -Parent $scriptPath)
Set-Location $projectRoot

Write-Host ""
Write-Host "Project root: $projectRoot" -ForegroundColor Cyan

# Clone Archipelago repository
if (-not $SkipArchipelago) {
    Write-Host ""
    Write-Host "Setting up Archipelago..." -ForegroundColor Yellow
    
    $apRepoPath = Join-Path $projectRoot "tools\Archipelago"
    
    if (Test-Path $apRepoPath) {
        Write-Host "✓ Archipelago repository already exists" -ForegroundColor Green
        Write-Host "  Updating repository..." -ForegroundColor Yellow
        Push-Location $apRepoPath
        git pull
        Pop-Location
    } else {
        Write-Host "  Cloning Archipelago repository..." -ForegroundColor Yellow
        New-Item -ItemType Directory -Path "tools" -Force | Out-Null
        git clone https://github.com/ArchipelagoMW/Archipelago.git $apRepoPath
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Archipelago cloned successfully" -ForegroundColor Green
        } else {
            Write-Host "✗ Failed to clone Archipelago" -ForegroundColor Red
            exit 1
        }
    }
}

# Set up Python virtual environment for APWorld development
Write-Host ""
Write-Host "Setting up Python environment..." -ForegroundColor Yellow

$venvPath = Join-Path $projectRoot "apworld\venv"

if (Test-Path $venvPath) {
    Write-Host "✓ Virtual environment already exists" -ForegroundColor Green
} else {
    Write-Host "  Creating virtual environment..." -ForegroundColor Yellow
    python -m venv $venvPath
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Virtual environment created" -ForegroundColor Green
    } else {
        Write-Host "✗ Failed to create virtual environment" -ForegroundColor Red
        exit 1
    }
}

# Install Python dependencies
Write-Host "  Installing Python dependencies..." -ForegroundColor Yellow
$activateScript = Join-Path $venvPath "Scripts\Activate.ps1"
& $activateScript

$requirementsPath = Join-Path $projectRoot "apworld\requirements.txt"
if (Test-Path $requirementsPath) {
    pip install -r $requirementsPath
} else {
    # Install basic dependencies
    pip install pytest pytest-cov black pylint
}

Write-Host "✓ Python dependencies installed" -ForegroundColor Green

# Configure game path
if ($GamePath -eq "") {
    Write-Host ""
    Write-Host "Game path configuration:" -ForegroundColor Yellow
    Write-Host "  Please enter the path to your Dave the Diver installation"
    Write-Host "  (Usually: C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver)"
    Write-Host ""
    $GamePath = Read-Host "Game path"
}

if ($GamePath -ne "" -and (Test-Path $GamePath)) {
    Write-Host "  Configuring game path..." -ForegroundColor Yellow
    
    $gamePropsPath = Join-Path $projectRoot "client\GamePath.props"
    $gamePropsContent = @"
<Project>
  <PropertyGroup>
    <GamePath>$GamePath</GamePath>
  </PropertyGroup>
</Project>
"@
    
    New-Item -ItemType Directory -Path "client" -Force | Out-Null
    Set-Content -Path $gamePropsPath -Value $gamePropsContent
    
    Write-Host "✓ Game path configured: $GamePath" -ForegroundColor Green
} elseif ($GamePath -ne "") {
    Write-Host "⚠ Game path not found: $GamePath" -ForegroundColor Yellow
    Write-Host "  You can configure it later by editing client/GamePath.props" -ForegroundColor Yellow
}

# Summary
Write-Host ""
Write-Host "=== Setup Complete! ===" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. Install BepInEx 6 IL2CPP to your game directory" -ForegroundColor White
Write-Host "     Download from: https://github.com/BepInEx/BepInEx/releases" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Run Dave the Diver once to generate interop assemblies" -ForegroundColor White
Write-Host ""
Write-Host "  3. Start developing!" -ForegroundColor White
Write-Host "     - APWorld (Python): cd apworld && .\venv\Scripts\Activate.ps1" -ForegroundColor Gray
Write-Host "     - Client (C#): cd client && dotnet build" -ForegroundColor Gray
Write-Host ""
Write-Host "  4. Read the documentation in docs/ folder" -ForegroundColor White
Write-Host ""
Write-Host "Happy coding! 🎮" -ForegroundColor Cyan
