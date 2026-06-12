# AsBuiltReport.Chart — Copilot Instructions

## Architecture Overview

This project is a **hybrid C# + PowerShell module**. The C# layer compiles into platform-specific DLLs; the PowerShell layer loads the right DLL at import time and exposes cmdlets.

```
Sources/                   ← C# library (ScottPlot + SkiaSharp, net8.0 / netstandard2.0)
  Chart.cs                 ← Static base class; all chart settings live here as static props
  PieChart.cs / DonutChart.cs / BarChart.cs / StackedBarChart.cs / SignalChart.cs / RadarChart.cs
                           ← Chart implementations
  PowerShell/              ← One PSCmdlet class per chart type (New-*Chart cmdlets)
  Enums/Enums.cs           ← All shared enums (BasicColors, Formats, ColorPalettes, etc.)

AsBuiltReport.Chart/       ← PowerShell module
  AsBuiltReport.Chart.psm1 ← Loads the correct DLL based on PSEdition + OS + architecture
  AsBuiltReport.Chart.psd1 ← Module manifest; exports: New-PieChart, New-DonutChart,
                             New-BarChart, New-StackedBarChart, New-SingleStackedBarChart,
                             New-SignalChart, New-RadarChart
  Src/Assemblies/          ← Pre-compiled DLLs, organized by platform:
    Core/linux-x64/
    Core/windows-x64/
    Core/mac-osx/osx-arm64/ and osx-x64/
    Desktop/windows-x64/   ← Windows PowerShell 5.1 (PSEdition Desktop)

Tests/
  AsBuiltReport.Chart.Functions.Tests.ps1   ← Pester v5 tests
  Invoke-Tests.ps1                          ← Test runner script
```

## Build & Test Commands

### Build the C# library (must rebuild after any Sources/ change)

```bash
# Linux
dotnet publish ./Sources -c Release -r linux-x64
cp Sources/bin/Release/netstandard2.0/linux-x64/publish/* AsBuiltReport.Chart/Src/Assemblies/Core/linux-x64/

# Windows (PowerShell 7)
dotnet publish ./Sources -c Release -r win-x64
Copy-Item ./Sources/bin/Release/netstandard2.0/win-x64/publish/* ./AsBuiltReport.Chart/Src/Assemblies/Core/windows-x64 -Recurse

# Windows PowerShell 5.1 (Desktop)
Copy-Item ./Sources/bin/Release/netstandard2.0/win-x64/publish/* ./AsBuiltReport.Chart/Src/Assemblies/Desktop/windows-x64 -Recurse

# macOS (Apple Silicon)
dotnet publish ./Sources -c Release -r osx-arm64
Copy-Item ./Sources/bin/Release/netstandard2.0/osx-arm64/publish/* ./AsBuiltReport.Chart/Src/Assemblies/Core/mac-osx/osx-arm64 -Recurse
```

### Run all Pester tests

```powershell
.\Tests\Invoke-Tests.ps1
```

### Run a single test (by name filter)

```powershell
$cfg = New-PesterConfiguration
$cfg.Run.Path = '.\Tests'
$cfg.Filter.FullName = '*New-PieChart*'
Invoke-Pester -Configuration $cfg
```

### Lint (PSScriptAnalyzer)

```powershell
Invoke-ScriptAnalyzer -Path . -Recurse -Settings .\.github\workflows\PSScriptAnalyzerSettings.psd1
```

Excluded rules: `PSUseToExportFieldsInManifest`, `PSReviewUnusedParameter`, `PSUseDeclaredVarsMoreThanAssignments`, `PSAvoidGlobalVars`.

### Build (Pester Tests) in CI/CD

```powershell
.\Tests\Invoke-Tests.ps1 -CodeCoverage -OutputFormat NUnitXml
```

Supports optional flags:
- `-CodeCoverage` – Enable code coverage analysis
- `-OutputFormat <Console|NUnitXml|JUnitXml>` – Output format for test results (default: Console)

## Key Conventions

### Cross-platform DLL loading

The `.psm1` module import process detects the current PowerShell edition, OS, and architecture, then loads the appropriate pre-compiled DLL:
- **PowerShell 7+** on Windows/Linux/macOS → Loads from `Core/` folder
- **PowerShell 5.1 (Desktop)** on Windows → Loads from `Desktop/windows-x64/` folder (Windows-only)

If the correct DLL cannot be found for the runtime, the module import fails with a clear error message.

### Adding a new chart type

Every new chart type requires changes in both layers:

1. **C# side** (`Sources/`): Add `<NewType>.cs` (chart logic) and `PowerShell/<NewType>Pwsh.cs` (the PSCmdlet class).
2. **PowerShell side**: Add the exported function name to `FunctionsToExport` in `AsBuiltReport.Chart.psd1`.
3. **Tests**: Add a `Context '<New-XChart>'` block in `Tests/AsBuiltReport.Chart.Functions.Tests.ps1`.

### Chart static-state pattern

`Chart` properties are **static**. Every `ProcessRecord()` override **must** call `Chart.Reset()` first to clear state from the previous invocation before setting new property values.

### Cross-platform path construction

Use `[System.IO.Path]::DirectorySeparatorChar` (not hardcoded `/` or `\`) when building paths in `.psm1` or C# code. The `.psm1` uses the format string pattern:

```powershell
"$PSScriptRoot{0}Src{0}Assemblies{0}..." -f [System.IO.Path]::DirectorySeparatorChar
```

### StackedBarChart Values layout

`-Values` accepts a **jagged array**: one inner `double[]` per bar/label. Wrapping a single array requires the unary comma operator to prevent flattening:

```powershell
New-StackedBarChart -Values @(,[double[]]@(1, 2)) -Labels @('A') -LegendCategories @('X','Y') ...
```

### OutputFolderPath validation

The `ValidatePath` custom attribute (defined in `PieChartPwsh.cs`) validates that the directory exists. Pass `-OutputFolderPath $TestDrive` in tests to use Pester's temp path.

### Chart cmdlet return value

All `New-*Chart` cmdlets return a `System.IO.FileSystemInfo` object (the saved file), not a string path. Tests assert `Should -BeOfType 'System.IO.FileSystemInfo'`.

### Branching

- Target PRs against the **`dev`** branch.
- Follow [Keep a Changelog](https://keepachangelog.com/) when updating `CHANGELOG.md`.
- Follow the [PowerShell Best Practices and Style Guide](https://github.com/PoshCode/PowerShellPracticeAndStyle).
- Use PascalCasing for all public member, type, and namespace names.
