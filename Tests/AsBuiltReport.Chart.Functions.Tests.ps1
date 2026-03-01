
# Requires -Version 5.0

BeforeAll {
    # Import the module
    $ModulePath = Join-Path -Path $PSScriptRoot -ChildPath '..\AsBuiltReport.Chart\AsBuiltReport.Chart.psd1'
    Import-Module $ModulePath -Force
}

Describe 'AsBuiltReport.Chart Exported Functions' {
    It 'Should export New-PieChart' {
        Get-Command -Module AsBuiltReport.Chart -Name New-PieChart | Should -Not -BeNullOrEmpty
    }
    It 'Should export New-BarChart' {
        Get-Command -Module AsBuiltReport.Chart -Name New-BarChart | Should -Not -BeNullOrEmpty
    }
    It 'Should export New-StackedBarChart' {
        Get-Command -Module AsBuiltReport.Chart -Name New-StackedBarChart | Should -Not -BeNullOrEmpty
    }
    It 'Should export New-SignalChart' {
        Get-Command -Module AsBuiltReport.Chart -Name New-SignalChart | Should -Not -BeNullOrEmpty
    }

    Context 'New-PieChart' {
        It 'Should run without error with sample input' {
            { New-PieChart -Title 'Test' -Values @(1, 2) -Labels @('A', 'B') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-PieChart -Title 'Test' -Values @(1, 2) -Labels @('A', 'B') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-PieChart } | Should -Throw
        }
        It 'Should throw error for mismatched Values and Labels' {
            { New-PieChart -Title 'Test' -Values @(1, 2) -Labels @('A') -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage  "Error: Values and labels must be equal."
        }
    }

    Context 'New-BarChart' {
        It 'Should run without error with sample input' {
            { New-BarChart -Title 'Test' -Values @(1, 2) -Labels @('A', 'B') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-BarChart -Title 'Test' -Values @(1, 2) -Labels @('A', 'B') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-BarChart } | Should -Throw
        }
        It 'Should throw error for mismatched Values and Labels' {
            { New-BarChart -Title 'Test' -Values @(1, 2) -Labels @('A') -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage  "Error: Values and labels must be equal."
        }
    }

    Context 'New-StackedBarChart' {
        It 'Should run without error with sample input' {
            { New-StackedBarChart -Title 'Test' -Values @(@(1, 2), @(3, 4)) -Labels @('A', 'B') -LegendCategories @('X', 'Y') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-StackedBarChart -Title 'Test' -Values @(@(1, 2), @(3, 4)) -Labels @('A', 'B') -LegendCategories @('X', 'Y') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-StackedBarChart } | Should -Throw
        }
        It 'Should throw error for mismatched Values and Labels' {
            { New-StackedBarChart -Title 'Test' -Values @(@(1, 2), @(3, 4)) -Labels @('A') -LegendCategories @('X', 'Y') -OutputFolderPath $TestDrive -Format 'png' } | Should -Throw -ExpectedMessage "Error: Values and labels must be equal."
        }
        It 'Should throw error for mismatched Values and LegendCategories' {
            { New-StackedBarChart -Title 'Test' -Values @(@(1, 2), @(3, 4)) -Labels @('A', 'B') -LegendCategories @('X') -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage "Error: Values and category names must be equal."
        }
        It 'Should run without error with a single element (single-bar chart)' {
            { New-StackedBarChart -Title 'Test' -Values @(,[double[]]@(1, 2)) -Labels @('A') -LegendCategories @('X', 'Y') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output for a single element' {
            $result = New-StackedBarChart -Title 'Test' -Values @(,[double[]]@(1, 2)) -Labels @('A') -LegendCategories @('X', 'Y') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
    }

    Context 'New-SignalChart' {
        It 'Should run without error with a single signal line' {
            { New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3, 4)) -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3, 4)) -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should run without error with multiple signal lines' {
            { New-SignalChart -Title 'Test' -Values @(@(1, 2, 3), @(4, 5, 6)) -Labels @('Line1', 'Line2') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should run without error with DateTime ticks' {
            $xOffset = (Get-Date '2024-01-01').ToOADate()
            { New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3)) -XOffset $xOffset -Period 1.0 -DateTimeTicksBottom -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should run without error in scatter mode with explicit X values' {
            $xValues = @(,[double[]]@(1.0, 2.0, 3.0, 4.0))
            { New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3, 4)) -ScatterXValues $xValues -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output in scatter mode' {
            $xValues = @(,[double[]]@(1.0, 2.0, 3.0, 4.0))
            $result = New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3, 4)) -ScatterXValues $xValues -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should run without error in scatter mode with DateTime X values' {
            $startDate = (Get-Date '2024-01-01').ToOADate()
            {
                $xArr = [double[]]@($startDate, ($startDate + 1.0), ($startDate + 2.0))
                New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3)) -ScatterXValues @(,$xArr) -DateTimeTicksBottom -Format 'png' -OutputFolderPath $TestDrive
            } | Should -Not -Throw
        }
        It 'Should run without error in scatter mode with multiple lines' {
            $xValues = @(@(1.0, 2.0, 3.0), @(1.0, 2.0, 3.0))
            { New-SignalChart -Title 'Test' -Values @(@(1, 2, 3), @(4, 5, 6)) -ScatterXValues $xValues -Labels @('Line1', 'Line2') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should throw error when ScatterXValues count does not match Values count' {
            $xValues = @(,[double[]]@(1.0, 2.0, 3.0))
            { New-SignalChart -Title 'Test' -Values @(@(1, 2, 3), @(4, 5, 6)) -ScatterXValues $xValues -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage "Error: XValues and Values must have the same number of arrays."
        }
        It 'Should throw error when XValues elements count does not match Values elements count' {
            $xValues = @(,[double[]]@(1.0, 2.0))
            { New-SignalChart -Title 'Test' -Values @(,[double[]]@(1, 2, 3)) -ScatterXValues $xValues -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage "Error: XValues and Values at index 0 must have the same number of elements."
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-SignalChart } | Should -Throw
        }
        It 'Should throw error when Values is null or empty' {
            { New-SignalChart -Title 'Test' -Values $null -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw
        }
        Context 'ONTAP-NAS day data (Scatter + DateTimeTicksBottom)' {
            BeforeAll {
                # Simulate ONTAP-NAS day data: 24 hourly data points for multiple NFS metrics
                $baseDate = (Get-Date '2024-01-01').ToOADate()
                $script:OntapXArr = [double[]](0..23 | ForEach-Object { $baseDate + ($_ / 24.0) })
                $script:OntapNfsRead = [double[]]@(10.2, 12.5, 8.7, 6.3, 5.1, 7.4, 11.8, 15.6, 18.2, 20.1, 22.4, 19.8, 17.3, 16.5, 18.9, 21.2, 23.6, 20.8, 17.4, 14.2, 12.1, 10.5, 9.3, 8.8)
                $script:OntapNfsWrite = [double[]]@(5.1, 6.2, 4.3, 3.1, 2.5, 3.7, 5.9, 7.8, 9.1, 10.0, 11.2, 9.9, 8.6, 8.2, 9.4, 10.6, 11.8, 10.4, 8.7, 7.1, 6.0, 5.2, 4.6, 4.4)
            }
            It 'Should run without error with ONTAP-NAS day data in scatter mode with DateTimeTicksBottom' {
                {
                    New-SignalChart `
                        -Title 'ONTAP-NAS NFS Throughput (Day)' `
                        -Values @($script:OntapNfsRead, $script:OntapNfsWrite) `
                        -ScatterXValues @($script:OntapXArr, $script:OntapXArr) `
                        -Labels @('NFS Read', 'NFS Write') `
                        -LabelXAxis 'Time' `
                        -LabelYAxis 'Throughput (MB/s)' `
                        -DateTimeTicksBottom `
                        -EnableLegend `
                        -Format 'png' `
                        -OutputFolderPath $TestDrive
                } | Should -Not -Throw
            }
            It 'Should return a file path for ONTAP-NAS day data chart' {
                $result = New-SignalChart `
                    -Title 'ONTAP-NAS NFS Throughput (Day)' `
                    -Values @($script:OntapNfsRead, $script:OntapNfsWrite) `
                    -ScatterXValues @($script:OntapXArr, $script:OntapXArr) `
                    -Labels @('NFS Read', 'NFS Write') `
                    -LabelXAxis 'Time' `
                    -LabelYAxis 'Throughput (MB/s)' `
                    -DateTimeTicksBottom `
                    -EnableLegend `
                    -Format 'png' `
                    -OutputFolderPath $TestDrive

                $result | Should -BeOfType 'System.IO.FileSystemInfo'
                Test-Path $result | Should -BeTrue
            }
        }
    }
}
