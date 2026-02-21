# Get assemblies files and import them
switch ($PSVersionTable.PSEdition) {
    'Core' {
        if ($IsMacOS) {
            $architecture = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture
            if ($architecture -eq "Arm64" -or $architecture -eq "Arm") {
                Write-Verbose "Architecture: ARM (Apple Silicon)"
                Import-Module ("$PSScriptRoot{0}Src{0}Assemblies{0}Core{0}mac-osx{0}arm{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar) -Verbose -Debug

            } elseif ($architecture -eq "X64" -or $architecture -eq "X86") {
                Write-Verbose "Architecture: x86 (Intel)"
                Import-Module ("$PSScriptRoot{0}Src{0}Assemblies{0}Core{0}mac-osx{0}x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar) -Verbose -Debug

            } else {
                Write-Verbose "Architecture: Unknown or other architecture"
                Import-Module ("$PSScriptRoot{0}Src{0}Assemblies{0}Core{0}mac-osx{0}arm{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)  -Verbose -Debug
            }
        } elseif ($IsLinux) {
            Import-Module ("$PSScriptRoot{0}Src{0}Assemblies{0}Core{0}linux-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
        } elseif ($IsWindows) {
            Import-Module ("$PSScriptRoot{0}Src{0}Assemblies{0}Core{0}windows-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
        }
    }
    'Desktop' {
        Import-Module ("$PSScriptRoot{0}Src{0}Assemblies{0}Desktop{0}windows-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
    }
    default {
        Write-Verbose -Message 'Unable to find compatible assemblies.'
    }
}