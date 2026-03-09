<#
    .SYNOPSIS
    Example 09 - Signal Chart with Multiple Lines

    .DESCRIPTION
    This example demonstrates how to create a Signal Chart with multiple lines and advanced visual
    options using the AsBuiltReport.Chart module.

    Features demonstrated:
    - Multiple signal lines on a single chart
    - Scatter mode with explicit X values and DateTime X-axis
    - Legend with custom font size
    - Custom color palette
    - Bold title and axis labels

    The chart displays hourly NFS read and write throughput for a NetApp ONTAP NAS volume over
    one day, simulating performance data that would typically come from a monitoring system.
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
    Define the time range for the X-axis.
    OADate (OLE Automation Date) values are used to represent DateTime as a floating-point number.
    Each increment of 1.0/24 represents one hour.
#>

$StartDate = (Get-Date '2024-06-01 00:00:00').ToOADate()
$HourCount = 24

# Build an array of OADate values spaced one hour apart (shared by all lines).
$XValues = [double[]](0..($HourCount - 1) | ForEach-Object { $StartDate + ($_ / 24.0) })

<#
    Hourly NFS throughput data in MB/s for a 24-hour window.
    In a real-world scenario these values would come from your storage monitoring system.
#>

$NfsRead  = [double[]]@(10.2, 12.5, 8.7,  6.3,  5.1,  7.4,  11.8, 15.6, 18.2, 20.1, 22.4, 19.8, 17.3, 16.5, 18.9, 21.2, 23.6, 20.8, 17.4, 14.2, 12.1, 10.5, 9.3, 8.8)
$NfsWrite = [double[]]@(5.1,  6.2,  4.3,  3.1,  2.5,  3.7,  5.9,  7.8,  9.1,  10.0, 11.2, 9.9,  8.6,  8.2,  9.4,  10.6, 11.8, 10.4, 8.7,  7.1,  6.0,  5.2,  4.6, 4.4)

# Each inner array is a separate line. The outer @() creates the list of series.
$Values      = @($NfsRead, $NfsWrite)
$Labels      = @('NFS Read', 'NFS Write')

# ScatterXValues must match $Values in structure: one X array per Y array.
$ScatterXValues = @($XValues, $XValues)

<#
    Custom colors allow signal lines to be visually distinguished.
    Colors are assigned in the same order as $Values.
#>

$CustomColors = @('#3498DB', '#E74C3C')

<#
    The New-SignalChart cmdlet generates the Signal Chart image.

    -Title                    : Sets the chart title displayed at the top of the image.
    -TitleFontSize            : Sets the font size of the title in points.
    -TitleFontBold            : Renders the title in bold.
    -Values                   : Array of double arrays. Each inner array is one signal line/series.
    -Labels                   : Array of label strings, one per series (shown in the legend).
    -ScatterXValues           : Array of double arrays containing OADate X values for each series.
    -DateTimeTicksBottom      : Formats the X-axis as human-readable date/time labels.
    -EnableLegend             : Enables the legend on the chart.
    -LegendOrientation        : Sets legend layout direction (Vertical or Horizontal).
    -LegendAlignment          : Positions the legend (e.g. UpperRight, LowerCenter).
    -LegendFontSize           : Sets the legend text font size in points.
    -LabelXAxis               : Label for the X-axis.
    -LabelYAxis               : Label for the Y-axis.
    -EnableCustomColorPalette : Enables use of the custom color palette.
    -CustomColorPalette       : Array of hex color strings, one per series.
    -Width                    : Output image width in pixels.
    -Height                   : Output image height in pixels.
    -Format                   : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath         : Directory where the generated chart file will be saved.
    -Filename                 : Name of the output file (without extension).
#>

New-SignalChart `
    -Title 'ONTAP NAS - NFS Throughput (MB/s)' `
    -TitleFontSize 18 `
    -TitleFontBold `
    -Values $Values `
    -Labels $Labels `
    -ScatterXValues $ScatterXValues `
    -DateTimeTicksBottom `
    -EnableLegend `
    -LegendOrientation Vertical `
    -LegendAlignment UpperRight `
    -LegendFontSize 12 `
    -LabelXAxis 'Time' `
    -LabelYAxis 'Throughput (MB/s)' `
    -EnableCustomColorPalette `
    -CustomColorPalette $CustomColors `
    -Width 800 `
    -Height 450 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example09-SignalChart-MultiLine'
