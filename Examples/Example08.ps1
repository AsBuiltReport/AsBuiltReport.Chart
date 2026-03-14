<#
    .SYNOPSIS
    Example 08 - Signal Chart with DateTime X-Axis (Time-Series Data)

    .DESCRIPTION
    This example demonstrates how to create a Signal Chart with a DateTime X-axis using the
    AsBuiltReport.Chart module.

    When the -DateTimeTicksBottom switch is used, the X-axis is formatted as human-readable
    date/time values. Data points are specified using OADate (OLE Automation Date) values which
    can be obtained by calling .ToOADate() on a DateTime object.

    The chart displays hourly NFS read throughput for a NetApp ONTAP NAS volume over one day.
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
    Define the time range for the X-axis.
    OADate (OLE Automation Date) values are used to represent DateTime as a floating-point number.
    Each increment of 1.0/24 represents one hour.
#>

$StartDate = (Get-Date '2024-06-01 00:00:00').ToOADate()
$HourCount = 24

# Build an array of OADate values spaced one hour apart.
$XValues = [double[]](0..($HourCount - 1) | ForEach-Object { $StartDate + ($_ / 24.0) })

<#
    Hourly NFS read throughput in MB/s for a 24-hour window.
    In a real-world scenario these values would come from your storage monitoring system.
#>

$NfsRead = [double[]]@(10.2, 12.5, 8.7, 6.3, 5.1, 7.4, 11.8, 15.6, 18.2, 20.1, 22.4, 19.8, 17.3, 16.5, 18.9, 21.2, 23.6, 20.8, 17.4, 14.2, 12.1, 10.5, 9.3, 8.8)
$Values  = @(,$NfsRead)

<#
    The New-SignalChart cmdlet generates the Signal Chart image.

    -Title                : Sets the chart title displayed at the top of the image.
    -Values               : Array of double arrays. Each inner array is one signal line/series.
    -ScatterXValues       : Array of double arrays containing OADate X values for each series.
                            When provided, scatter mode is used (explicit X positions per point).
    -DateTimeTicksBottom  : Formats the X-axis as human-readable date/time labels.
    -LabelXAxis           : Label for the X-axis.
    -LabelYAxis           : Label for the Y-axis.
    -Width                : Output image width in pixels.
    -Height               : Output image height in pixels.
    -Format               : Output file format (e.g. png, jpg, svg).
    -OutputFolderPath     : Directory where the generated chart file will be saved.
    -Filename             : Name of the output file (without extension).
#>

New-SignalChart `
    -Title 'ONTAP NAS - NFS Read Throughput (MB/s)' `
    -Values $Values `
    -ScatterXValues @(,$XValues) `
    -DateTimeTicksBottom `
    -LabelXAxis 'Time' `
    -LabelYAxis 'Throughput (MB/s)' `
    -Width 700 `
    -Height 400 `
    -Format $Format `
    -OutputFolderPath $OutputFolderPath `
    -Filename 'Example08-SignalChart-DateTime'
