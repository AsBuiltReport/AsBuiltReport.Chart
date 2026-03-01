
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
    }
}
