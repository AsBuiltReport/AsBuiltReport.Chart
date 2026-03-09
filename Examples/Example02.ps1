<#
    .SYNOPSIS
    Example 02 - Pie Chart with Legend, Custom Colors and Border

    .DESCRIPTION
    This example demonstrates how to create a Pie Chart with additional visual options, including:
    - An enabled legend with custom alignment and orientation
    - A custom hex color palette
    - A chart border
    - Adjusted title and label font sizes
    - Custom chart dimensions

    The chart displays a server operating system distribution report.
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

$ChartTitle  = 'Server OS Distribution'
$Values      = @(85, 60, 30, 15)
$Labels      = @('Windows Server 2022', 'Windows Server 2019', 'RHEL 9', 'Ubuntu 22.04')

<#
    A custom hex color palette can be used to match corporate branding or improve readability.
    Each color corresponds to a slice in the order they appear in $Values.
#>

$CustomColors = @('#0078D4', '#00B7C3', '#E74C3C', '#F39C12')

<#
    The New-PieChart cmdlet generates the Pie Chart image.

    -Title                    : Sets the chart title.
    -TitleFontSize            : Sets the font size of the title in points.
    -TitleFontBold            : Renders the title in bold.
    -Values                   : Array of numeric values, one per slice.
    -Labels                   : Array of label strings corresponding to each value.
    -LabelFontSize            : Sets the font size of the slice labels.
    -LabelDistance            : Controls how far labels are placed from the chart center (0.5-0.9).
    -EnableLegend             : Enables the legend on the chart.
    -LegendAlignment          : Positions the legend (e.g. UpperRight, LowerCenter).
    -LegendOrientation        : Sets legend layout direction (Vertical or Horizontal).
    -LegendFontSize           : Sets the legend text font size in points.
    -EnableChartBorder        : Draws a border around the chart area.
    -ChartBorderColor         : Sets the border color.
    -ChartBorderSize          : Sets the border thickness in pixels.
    -EnableCustomColorPalette : Enables use of the custom color palette.
    -CustomColorPalette       : Array of hex color strings for each slice.
    -Width                    : Output image width in pixels.
    -Height                   : Output image height in pixels.
    -Format                   : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath         : Directory where the generated chart file will be saved.
    -Filename                 : Name of the output file (without extension).
#>

New-PieChart `
    -Title $ChartTitle `
    -TitleFontSize 18 `
    -TitleFontBold `
    -Values $Values `
    -Labels $Labels `
    -LabelFontSize 13 `
    -LabelDistance 0.7 `
    -EnableLegend `
    -LegendAlignment UpperRight `
    -LegendOrientation Vertical `
    -LegendFontSize 12 `
    -EnableChartBorder `
    -ChartBorderColor Black `
    -ChartBorderSize 2 `
    -EnableCustomColorPalette `
    -CustomColorPalette $CustomColors `
    -Width 600 `
    -Height 400 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example02-PieChart-Advanced'
