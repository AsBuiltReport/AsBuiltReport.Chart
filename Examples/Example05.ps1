<#
    .SYNOPSIS
    Example 05 - Basic Stacked Bar Chart

    .DESCRIPTION
    This example demonstrates how to create a basic Stacked Bar Chart using the AsBuiltReport.Chart module.
    The chart displays datastore utilization (used vs. free space) for a set of vSphere datastores.
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

    For a Stacked Bar Chart:
    - $Values is an array of double arrays. Each inner array contains the values for one category
      (stack segment) across all bars.
    - $Labels contains the label for each bar (X-axis entries).
    - $LegendCategories contains the label for each series (stack segments shown in the legend).

    In this example:
    - Bar 1 = datastore-01: 800 GB Used, 200 GB Free
    - Bar 2 = datastore-02: 600 GB Used, 400 GB Free
    - Bar 3 = datastore-03: 1500 GB Used, 500 GB Free
    - Bar 4 = datastore-04: 300 GB Used, 700 GB Free
#>

$ChartTitle = 'Datastore Capacity (GB)'
$Labels = @('datastore-01', 'datastore-02', 'datastore-03', 'datastore-04')
$LegendCategories = @('Used Space', 'Free Space')

# Each inner array represents one category's values across all bars.
$Values = @(@(800, 200), @(600, 400), @(1500, 500), @(300, 700))

<#
    The New-StackedBarChart cmdlet generates the Stacked Bar Chart image.

    -Title              : Sets the chart title displayed at the top of the image.
    -Values             : Array of double arrays. Each inner array is one stack segment across all bars.
    -Labels             : Array of label strings, one per bar (X-axis entries).
    -LegendCategories   : Array of category names shown in the legend (one per inner array in Values).
    -EnableLegend       : Enables the legend on the chart.
    -LabelXAxis         : Label for the X-axis.
    -LabelYAxis         : Label for the Y-axis.
    -Format             : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath   : Directory where the generated chart file will be saved.
    -Width              : Width of the output image in pixels.
    -Height             : Height of the output image in pixels.
    -AreaOrientation    : Orientation of the bars (Horizontal or Vertical).
    -Filename           : Name of the output file (without extension).
#>

New-StackedBarChart `
    -Title $ChartTitle `
    -Values $Values `
    -Labels $Labels `
    -LegendCategories $LegendCategories `
    -EnableLegend `
    -LabelXAxis 'Datastore' `
    -LabelYAxis 'Capacity (GB)' `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Width 700 `
    -Height 450 `
    -AreaOrientation Horizontal `
    -Filename 'Example05-StackedBarChart'
