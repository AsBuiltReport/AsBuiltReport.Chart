function Get-ChartAssemblyPath {
    $separator = [System.IO.Path]::DirectorySeparatorChar
    $assemblyRoot = "$PSScriptRoot{0}Src{0}Assemblies" -f $separator

    switch ($PSVersionTable.PSEdition) {
        'Core' {
            if ($IsMacOS) {
                $architecture = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture

                if ($architecture -in @('Arm64', 'Arm')) {
                    Write-Verbose 'Architecture: ARM (Apple Silicon)'
                    $path = "$assemblyRoot{0}Core{0}mac-osx{0}osx-arm64{0}AsBuiltReportChart.dll" -f $separator
                }
                elseif ($architecture -in @('X64', 'X86')) {
                    Write-Verbose 'Architecture: x86 (Intel)'
                    $path = "$assemblyRoot{0}Core{0}mac-osx{0}osx-x64{0}AsBuiltReportChart.dll" -f $separator
                }
                else {
                    throw "Unsupported macOS architecture '$architecture'. Supported architectures: Arm64, X64, X86."
                }
            }
            elseif ($IsLinux) {
                Write-Verbose 'Architecture: Linux x64'
                $path = "$assemblyRoot{0}Core{0}linux-x64{0}AsBuiltReportChart.dll" -f $separator
            }
            elseif ($IsWindows) {
                Write-Verbose 'Architecture: Windows x64'
                $path = "$assemblyRoot{0}Core{0}windows-x64{0}AsBuiltReportChart.dll" -f $separator
            }
            else {
                throw 'Unable to determine a supported platform for the Core PowerShell edition.'
            }
        }
        'Desktop' {
            Write-Verbose 'Architecture: Windows x64 (PowerShell 5.1)'
            $path = "$assemblyRoot{0}Desktop{0}windows-x64{0}AsBuiltReportChart.dll" -f $separator
        }
        default {
            throw "Unsupported PowerShell edition '$($PSVersionTable.PSEdition)'. Supported editions: Core, Desktop."
        }
    }

    if (-not (Test-Path -LiteralPath $path)) {
        throw "Unable to locate the AsBuiltReportChart assembly for this environment: $path"
    }

    return $path
}

$assemblyPath = Get-ChartAssemblyPath
Import-Module -Name $assemblyPath -Force