<#
    .SYNOPSIS
    Example 16 - Heat Map Chart

    .DESCRIPTION
    This example demonstrates how to create a Heat Map Chart using the AsBuiltReport.Chart module.
    The chart displays cluster utilization across resource types for multiple clusters.
#>

[CmdletBinding()]
param (
    [System.IO.DirectoryInfo] $Path = (Get-Location).Path,
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

$ChartTitle = 'Cluster Utilization'
$Values = @(
    @(78, 81, 74),
    @(66, 72, 80),
    @(88, 91, 85)
)
$RowLabels = @('Cluster A', 'Cluster B', 'Cluster C')
$ColumnLabels = @('CPU', 'Memory', 'Storage')

<#
    The New-HeatMapChart cmdlet generates the Heat Map image.

    -Title              : Sets the chart title displayed at the top of the image.
    -Values             : 2D matrix of numeric values for the heat map.
    -RowLabels          : Labels for each row in the heat map.
    -ColumnLabels       : Labels for each column in the heat map.
    -Format             : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath   : Directory where the generated chart file will be saved.
    -Width              : Width of the chart image in pixels.
    -Height             : Height of the chart image in pixels.
    -Filename           : Name of the output file (without extension).
    -EnableLegend       : Boolean to enable or disable the legend on the chart.
#>

New-HeatMapChart `
    -Title $ChartTitle `
    -Values $Values `
    -RowLabels $RowLabels `
    -ColumnLabels $ColumnLabels `
    -LabelXAxis 'Resources' `
    -LabelYAxis 'Clusters' `
    -EnableLegend `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Width 700 `
    -Height 500 `
    -Filename 'Example16-HeatMapChart'
