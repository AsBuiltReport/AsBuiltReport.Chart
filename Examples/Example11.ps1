<#
    .SYNOPSIS
    Example 11 - Single Stacked Bar Chart with Advanced Options

    .DESCRIPTION
    This example demonstrates how to create a Single Stacked Bar Chart with advanced visual options
    using the AsBuiltReport.Chart module.

    Features demonstrated:
    - Horizontal bar orientation
    - Custom hex color palette
    - Bold title with larger font
    - Chart border
    - Legend with custom font size

    The chart displays a VMware vSphere cluster resource pool CPU entitlement breakdown showing
    how the total CPU allocation is distributed across workload pools:
    - Production   : 45 %
    - Development  : 20 %
    - Testing      : 15 %
    - Management   : 12 %
    - Reserve      : 8  %
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

    When -AreaOrientation is set to Horizontal:
    - The bar grows left-to-right.
    - The value axis (X-axis) is formatted as percentages.
    - The category axis (Y-axis) shows the $Label string.
#>

$ChartTitle = 'vSphere Cluster - CPU Entitlement by Resource Pool (%)'
$Label = 'Cluster-Prod'
$LegendCategories = @('Production', 'Development', 'Testing', 'Management', 'Reserve')
$Values = [double[]]@(45, 20, 15, 12, 8)

<#
    A custom hex color palette gives each segment a distinct colour that can be aligned with
    corporate branding or infrastructure classification conventions.
    Colors are assigned in the same order as $LegendCategories.
#>

$CustomColors = @('#2E86AB', '#A23B72', '#F18F01', '#C73E1D', '#3B1F2B')

<#
    The New-SingleStackedBarChart cmdlet generates the Single Stacked Bar Chart image.

    -Title                    : Sets the chart title displayed at the top of the image.
    -TitleFontSize            : Sets the font size of the title in points.
    -TitleFontBold            : Renders the title in bold.
    -Values                   : Flat double array — one value per segment, expressed as a percentage.
    -Label                    : String label for the single bar on the category axis.
    -LegendCategories         : Array of segment names shown in the legend.
    -EnableLegend             : Enables the legend on the chart.
    -LegendOrientation        : Sets legend layout direction (Vertical or Horizontal).
    -LegendAlignment          : Positions the legend (e.g. LowerCenter, UpperRight).
    -LegendFontSize           : Sets the legend text font size in points.
    -LabelFontSize            : Sets the font size of the axis and segment labels.
    -AreaOrientation          : Vertical (default) renders the bar growing upward; Horizontal grows right.
    -EnableChartBorder        : Draws a border around the entire chart figure.
    -ChartBorderColor         : Color of the chart border.
    -EnableCustomColorPalette : Enables use of the custom color palette.
    -CustomColorPalette       : Array of hex color strings, one per segment.
    -Width                    : Output image width in pixels.
    -Height                   : Output image height in pixels.
    -Format                   : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath         : Directory where the generated chart file will be saved.
    -Filename                 : Name of the output file (without extension).
    -ValueSuffix              : Appends a suffix to the segment labels and value axis labels (e.g. '%').
#>

New-SingleStackedBarChart `
    -Title $ChartTitle `
    -TitleFontSize 16 `
    -TitleFontBold `
    -Values $Values `
    -Label $Label `
    -LegendCategories $LegendCategories `
    -EnableLegend `
    -LegendOrientation Horizontal `
    -LegendAlignment LowerCenter `
    -LegendFontSize 12 `
    -LabelFontSize 13 `
    -AreaOrientation Horizontal `
    -EnableChartBorder `
    -ChartBorderColor DarkBlue `
    -EnableCustomColorPalette `
    -CustomColorPalette $CustomColors `
    -Width 700 `
    -Height 300 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example11-SingleStackedBarChart-Advanced' `
    -ValueSuffix '%'
