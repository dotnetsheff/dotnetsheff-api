New-Item -ItemType Directory -Force -Path c:\downloads
Invoke-WebRequest -Uri https://visualstudiogallery.msdn.microsoft.com/e3705d94-7cc3-4b79-ba7b-f43f30774d28/file/263230/9/Microsoft.VisualStudio.Web.Azure.AzureFunctions.vsix -OutFile c:\downloads\Microsoft.VisualStudio.Web.Azure.AzureFunctions.vsix
$vswherePath = Join-Path ${env:ProgramFiles(x86)} '\Microsoft Visual Studio\Installer\vswhere.exe'
$vsInstallPath = & $vswherePath -latest -format value -property installationPath
$vsixInstallerPath = Join-Path $vsInstallPath '\Common7\IDE\VSIXInstaller.exe'
& $vsixInstallerPath /quiet c:\downloads\Microsoft.VisualStudio.Web.Azure.AzureFunctions.vsix

dotnet restore