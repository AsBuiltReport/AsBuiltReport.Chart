<#
    .SYNOPSIS
    Example 03 - Basic Bar Chart

    .DESCRIPTION
    This example demonstrates how to create a basic Bar Chart using the AsBuiltReport.Chart module.
    The chart displays CPU utilization across a set of ESXi hosts.
#>

[CmdletBinding()]
param (
    [System.IO.FileInfo] $Path = (Get-Location).Path,
    [string] $Format = 'png'
)

<#
    Starting with PowerShell v3, modules are auto-imported when needed. Importing the module here
    ensures clarity and avoids ambiguity.
#>

# Import-Module AsBuiltReport.Chart -Force -Verbose:$false

<#
    Since the chart output is a file, specify the output folder path using $OutputFolderPath.
#>

$OutputFolderPath = Resolve-Path $Path

<#
    Define the data to be displayed in the chart.
    In a real-world scenario these values would come from your infrastructure query.
#>

$ChartTitle  = 'ESXi Host CPU Utilization (%)'
$Values      = @(72, 45, 88, 61, 53)
$Labels      = @('esxi-host-01', 'esxi-host-02', 'esxi-host-03', 'esxi-host-04', 'esxi-host-05')

<#
    The New-BarChart cmdlet generates the Bar Chart image.

    -Title              : Sets the chart title displayed at the top of the image.
    -Values             : Array of numeric values, one per bar.
    -Labels             : Array of label strings corresponding to each bar.
    -LabelXAxis         : Label for the X-axis.
    -LabelYAxis         : Label for the Y-axis.
    -Format             : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath   : Directory where the generated chart file will be saved.
    -Filename           : Name of the output file (without extension).
#>

New-BarChart `
    -Title $ChartTitle `
    -Values $Values `
    -Labels $Labels `
    -LabelXAxis 'Host' `
    -LabelYAxis 'CPU (%)' `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example03-BarChart'
