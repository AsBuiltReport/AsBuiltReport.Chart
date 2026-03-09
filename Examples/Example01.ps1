<#
    .SYNOPSIS
    Example 01 - Basic Pie Chart

    .DESCRIPTION
    This example demonstrates how to create a basic Pie Chart using the AsBuiltReport.Chart module.
    The chart displays a simple breakdown of VM power states across a vSphere environment.
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

$ChartTitle  = 'VM Power States'
$Values      = @(120, 35, 10)
$Labels      = @('Powered On', 'Powered Off', 'Suspended')

<#
    The New-PieChart cmdlet generates the Pie Chart image.

    -Title              : Sets the chart title displayed at the top of the image.
    -Values             : Array of numeric values, one per slice.
    -Labels             : Array of label strings corresponding to each value.
    -Format             : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath   : Directory where the generated chart file will be saved.
    -Width              : Width of the chart image in pixels.
    -Height             : Height of the chart image in pixels.
    -ColorPalette       : Predefined color palette (e.g. Category20, Pastel
    -Filename           : Name of the output file (without extension).
#>

New-PieChart `
    -Title $ChartTitle `
    -Values $Values `
    -Labels $Labels `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Width 600 `
    -Height 400 `
    -ColorPalette Category20 `
    -Filename 'Example01-PieChart'
