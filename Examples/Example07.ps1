<#
    .SYNOPSIS
    Example 07 - Basic Signal Chart (Line Chart)

    .DESCRIPTION
    This example demonstrates how to create a basic Signal Chart (line chart) using the
    AsBuiltReport.Chart module.

    The Signal Chart is used to visualize sequential data points as a continuous line. In this
    example, a single line representing hourly CPU utilization over a 24-hour period is plotted.
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

    For a Signal Chart:
    - $Values is an array of double arrays. Each inner array represents a single line/series.
    - A single inner array produces a single line on the chart.

    In this example, 24 hourly CPU utilization samples are plotted as a single line.
    Data points are evenly spaced along the X-axis (using the default Period of 1.0).
#>

$ChartTitle = 'CPU Utilization - Last 24 Hours (%)'
$CpuData    = [double[]]@(12, 18, 15, 10, 8, 11, 25, 42, 65, 71, 68, 72, 74, 70, 66, 63, 58, 55, 48, 40, 35, 28, 22, 16)
$Values     = @(,$CpuData)

<#
    The New-SignalChart cmdlet generates the Signal Chart image.

    -Title              : Sets the chart title displayed at the top of the image.
    -Values             : Array of double arrays. Each inner array is one signal line/series.
    -LabelXAxis         : Label for the X-axis.
    -LabelYAxis         : Label for the Y-axis.
    -Format             : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath   : Directory where the generated chart file will be saved.
    -Filename           : Name of the output file (without extension).
#>

New-SignalChart `
    -Title $ChartTitle `
    -Values $Values `
    -LabelXAxis 'Hour' `
    -LabelYAxis 'CPU (%)' `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example07-SignalChart'
