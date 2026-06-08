<#
    .SYNOPSIS
    Example 10 - Basic Single Stacked Bar Chart

    .DESCRIPTION
    This example demonstrates how to create a Single Stacked Bar Chart using the
    AsBuiltReport.Chart module.

    A Single Stacked Bar Chart displays one bar divided into stacked segments, each representing
    a percentage contribution to the total. It is ideal for visualising how a whole is broken
    down into parts — for example, storage capacity usage across different categories.

    The chart displays a vSAN datastore capacity breakdown as a percentage of total raw capacity:
    - VM Data       : 55 %
    - Snapshots     : 15 %
    - Deduplication : 10 %
    - Overhead      : 12 %
    - Free          : 8  %
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

    For a Single Stacked Bar Chart:
    - $Values is a flat double array. Each element is the percentage value of one segment.
    - $Label is a single string that labels the one bar on the category axis.
    - $LegendCategories is an array of strings, one name per segment, shown in the legend.

    In this example, the segment labels rendered inside the bar include the '%' suffix
    because `-ValueSuffix '%'` is specified when the chart is created.
    The value axis (Y-axis for Vertical, X-axis for Horizontal) is also formatted as percentages.
#>

$ChartTitle = 'vSAN Datastore - Capacity Breakdown (%)'
$Label = 'Total'
$LegendCategories = @('VM Data', 'Snapshots', 'Deduplication', 'Overhead', 'Free')
$Values = [double[]]@(55, 15, 10, 12, 8)

<#
    The New-SingleStackedBarChart cmdlet generates the Single Stacked Bar Chart image.

    -Title              : Sets the chart title displayed at the top of the image.
    -Values             : Flat double array — one value per segment, expressed as a percentage.
    -Label              : String label for the single bar on the category axis.
    -LegendCategories   : Array of segment names shown in the legend.
    -EnableLegend       : Enables the legend on the chart.
    -LegendOrientation  : Sets legend layout direction (Vertical or Horizontal).
    -LegendAlignment    : Positions the legend (e.g. LowerCenter, UpperRight).
    -LegendFontSize     : Sets the legend text font size in points.
    -LabelFontSize      : Sets the font size of the axis and segment labels.
    -AreaOrientation    : Vertical (default) renders the bar growing upward; Horizontal grows right.
    -Width              : Output image width in pixels.
    -Height             : Output image height in pixels.
    -Format             : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath   : Directory where the generated chart file will be saved.
    -Filename           : Name of the output file (without extension).
    -ValueSuffix        : Appends a suffix to the segment labels and value axis labels (e.g. '%').
#>

New-SingleStackedBarChart `
    -Title $ChartTitle `
    -Values $Values `
    -Label $Label `
    -LegendCategories $LegendCategories `
    -EnableLegend `
    -LegendOrientation Horizontal `
    -LegendAlignment LowerCenter `
    -LegendFontSize 12 `
    -LabelFontSize 13 `
    -AreaOrientation Vertical `
    -Width 400 `
    -Height 500 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example10-SingleStackedBarChart' `
    -ValueSuffix '%'
