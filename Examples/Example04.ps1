<#
    .SYNOPSIS
    Example 04 - Bar Chart with Advanced Options

    .DESCRIPTION
    This example demonstrates how to create a Bar Chart with advanced options, including:
    - Horizontal bar orientation
    - Custom color palette
    - Bold title and labels
    - Custom axis labels and chart dimensions
    - Chart border

    The chart displays memory utilization across a set of physical servers.
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

$ChartTitle  = 'Physical Server Memory Utilization (%)'
$Values      = @(91, 67, 78, 55, 83, 44)
$Labels      = @('srv-prod-01', 'srv-prod-02', 'srv-prod-03', 'srv-dev-01', 'srv-dev-02', 'srv-test-01')

<#
    A custom hex color palette can be used to match corporate branding or improve readability.
    Each color corresponds to a bar in the order it appears in $Values.
#>

$CustomColors = @('#E74C3C', '#E67E22', '#F1C40F', '#2ECC71', '#3498DB', '#9B59B6')

<#
    The New-BarChart cmdlet generates the Bar Chart image.

    -Title                    : Sets the chart title.
    -TitleFontSize            : Sets the font size of the title in points.
    -TitleFontBold            : Renders the title in bold.
    -Values                   : Array of numeric values, one per bar.
    -Labels                   : Array of label strings corresponding to each bar.
    -LabelFontSize            : Sets the font size of the bar labels.
    -LabelBold                : Renders the bar labels in bold.
    -LabelXAxis               : Label for the X-axis.
    -LabelYAxis               : Label for the Y-axis.
    -AreaOrientation          : Orientation of the bars (Horizontal or Vertical).
    -AxesMarginsTop           : Top margin for the chart area as a fraction (0-1).
    -EnableChartBorder        : Draws a border around the chart area.
    -ChartBorderColor         : Sets the border color.
    -EnableCustomColorPalette : Enables use of the custom color palette.
    -CustomColorPalette       : Array of hex color strings for each bar.
    -Width                    : Output image width in pixels.
    -Height                   : Output image height in pixels.
    -Format                   : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath         : Directory where the generated chart file will be saved.
    -Filename                 : Name of the output file (without extension).
#>

New-BarChart `
    -Title $ChartTitle `
    -TitleFontSize 16 `
    -TitleFontBold `
    -Values $Values `
    -Labels $Labels `
    -LabelFontSize 12 `
    -LabelBold `
    -LabelXAxis 'Memory (%)' `
    -LabelYAxis 'Server' `
    -AreaOrientation Horizontal `
    -AxesMarginsTop 1 `
    -EnableChartBorder `
    -ChartBorderColor Black `
    -EnableCustomColorPalette `
    -CustomColorPalette $CustomColors `
    -Width 700 `
    -Height 450 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example04-BarChart-Advanced'
