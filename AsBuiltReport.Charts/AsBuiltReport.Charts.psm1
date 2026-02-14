# Get assemblies files and import them
switch ($PSVersionTable.PSEdition) {
    'Core' {
        if ($IsMacOS) {
            Import-Module ("$PSScriptRoot{0}Src{0}Bin{0}osx-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
        } elseif ($IsLinux) {
            Import-Module ("$PSScriptRoot{0}Src{0}Bin{0}linux-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
        } elseif ($IsWindows) {
            Import-Module ("$PSScriptRoot{0}Src{0}Bin{0}windows-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
        }
    }
    'Desktop' {
            Import-Module ("$PSScriptRoot{0}Src{0}Bin{0}windows-x64{0}AsBuiltReportChart.dll" -f [System.IO.Path]::DirectorySeparatorChar)
    }
    default {
        Write-Verbose -Message 'Unable to find compatible assemblies.'
    }
}