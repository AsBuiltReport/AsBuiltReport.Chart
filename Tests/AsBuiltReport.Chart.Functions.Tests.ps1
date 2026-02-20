
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

    Context 'New-PieChart' {
        It 'Should run without error with sample input' {
            { New-PieChart -Title 'Test' -Values @(1,2) -Labels @('A','B') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-PieChart -Title 'Test' -Values @(1,2) -Labels @('A','B') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-PieChart } | Should -Throw
        }
        It 'Should throw error for mismatched Values and Labels' {
            { New-PieChart -Title 'Test' -Values @(1,2) -Labels @('A') -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage  "Error: Values and labels must be equal."
        }
    }

    Context 'New-BarChart' {
        It 'Should run without error with sample input' {
            { New-BarChart -Title 'Test' -Values @(1,2) -Labels @('A','B') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-BarChart -Title 'Test' -Values @(1,2) -Labels @('A','B') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-BarChart } | Should -Throw
        }
        It 'Should throw error for mismatched Values and Labels' {
            { New-BarChart -Title 'Test' -Values @(1,2) -Labels @('A') -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage  "Error: Values and labels must be equal."
        }
    }

    Context 'New-StackedBarChart' {
        It 'Should run without error with sample input' {
            { New-StackedBarChart -Title 'Test' -Values @(@(1,2),@(3,4)) -Labels @('A','B') -LegendCategories @('X','Y') -Format 'png' -OutputFolderPath $TestDrive } | Should -Not -Throw
        }
        It 'Should return a file path as output' {
            $result = New-StackedBarChart -Title 'Test' -Values @(@(1,2),@(3,4)) -Labels @('A','B') -LegendCategories @('X','Y') -Format 'png' -OutputFolderPath $TestDrive
            $result | Should -BeOfType 'System.IO.FileSystemInfo'
            Test-Path $result | Should -BeTrue
        }
        It 'Should throw error for missing mandatory parameters' {
            { New-StackedBarChart } | Should -Throw
        }
        It 'Should throw error for mismatched Values and Labels' {
            { New-StackedBarChart -Title 'Test' -Values @(@(1,2),@(3,4)) -Labels @('A') -LegendCategories @('X','Y') -OutputFolderPath $TestDrive -Format 'png' } | Should -Throw -ExpectedMessage "Error: Values and labels must be equal."
        }
        It 'Should throw error for mismatched Values and LegendCategories' {
            { New-StackedBarChart -Title 'Test' -Values @(@(1,2),@(3,4)) -Labels @('A','B') -LegendCategories @('X') -Format 'png' -OutputFolderPath $TestDrive } | Should -Throw -ExpectedMessage "Error: Values and category names must be equal."
        }
    }
}
