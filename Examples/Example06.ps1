<#
    .SYNOPSIS
    Example 06 - Stacked Bar Chart with Advanced Options

    .DESCRIPTION
    This example demonstrates how to create a Stacked Bar Chart with advanced options, including:
    - Custom color palette
    - Horizontal legend alignment at the top of the chart
    - Bold title with larger font
    - Custom axis labels
    - Larger chart dimensions

    The chart displays network traffic breakdown (Inbound, Outbound, Dropped) per network adapter.
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
    - $Values is an array of double arrays. Each inner array contains the values for all categories
      (stack segments) for a single bar.
    - $Labels contains the label for each bar (X-axis entries).
    - $LegendCategories contains the label for each series (stack segments shown in the legend).

    In this example each bar represents a network adapter, and each stack segment is a traffic type.
#>

$ChartTitle = 'Network Adapter Traffic (Mbps)'
$Labels = @('vmnic0', 'vmnic1', 'vmnic2', 'vmnic3')
$LegendCategories = @('Inbound', 'Outbound', 'Dropped')

# Values are organized by bar (network adapter), so each inner array corresponds to one adapter and
# contains the values for each traffic type (stack segment) in the order of $LegendCategories.
# Example values for Inbound, Outbound, and Dropped traffic for each network adapter:
# - vmnic0: 450 Mbps Inbound, 320 Mbps Outbound, 280 Mbps Dropped
$vmnic0 = [double[]]@(450, 320, 280)
$vmnic1 = [double[]]@(380, 290, 210)
$vmnic2 = [double[]]@(150, 50, 300)
$vmnic3 = [double[]]@(200, 320, 300)
$Values = @($vmnic0, $vmnic1, $vmnic2, $vmnic3)

<#
    A custom hex color palette can be used to match corporate branding or improve readability.
    Each color corresponds to a stack segment category in the order they appear in $LegendCategories.
#>

$CustomColors = @('#3498DB', '#2ECC71', '#E74C3C')

<#
    The New-StackedBarChart cmdlet generates the Stacked Bar Chart image.

    -Title                    : Sets the chart title.
    -TitleFontSize            : Sets the font size of the title in points.
    -TitleFontBold            : Renders the title in bold.
    -Values                   : Array of double arrays, one per stack segment category.
    -Labels                   : Array of label strings, one per bar (X-axis entries).
    -LegendCategories         : Array of category names shown in the legend.
    -EnableLegend             : Enables the legend on the chart.
    -LegendOrientation        : Sets legend layout direction (Vertical or Horizontal).
    -LegendAlignment          : Positions the legend (e.g. UpperCenter, UpperRight).
    -LegendFontSize           : Sets the legend text font size in points.
    -LabelFontSize            : Sets the font size of the axis labels.
    -LabelXAxis               : Label for the X-axis.
    -LabelYAxis               : Label for the Y-axis.
    -AxesMarginsTop           : Top margin for the chart area as a fraction (0-1).
    -EnableCustomColorPalette : Enables use of the custom color palette.
    -CustomColorPalette       : Array of hex color strings, one per stack segment category.
    -Width                    : Output image width in pixels.
    -Height                   : Output image height in pixels.
    -Format                   : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath         : Directory where the generated chart file will be saved.
    -Filename                 : Name of the output file (without extension).
#>

New-StackedBarChart `
    -Title $ChartTitle `
    -TitleFontSize 18 `
    -TitleFontBold `
    -Values $Values `
    -Labels $Labels `
    -LegendCategories $LegendCategories `
    -EnableLegend `
    -LegendOrientation Horizontal `
    -LegendAlignment UpperCenter `
    -LegendFontSize 12 `
    -LabelFontSize 13 `
    -LabelXAxis 'Network Adapter' `
    -LabelYAxis 'Traffic (Mbps)' `
    -AxesMarginsTop 1 `
    -EnableCustomColorPalette `
    -CustomColorPalette $CustomColors `
    -Width 700 `
    -Height 450 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example06-StackedBarChart-Advanced'
