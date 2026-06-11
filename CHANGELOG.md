# :arrows_clockwise: AsBuiltReport Chart Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.3.3] - Unreleased

### Added

- Add support for Donut from Slices chart: [Donut from Slices](https://scottplot.net/cookbook/5/Pie/PieDonut/)
  - Add pester test to validate the functionality of the New-DonutChart cmdlet
  - Add example 12/13 to document on how to use the New-DonutChart cmdlet
- Add radar chart support: implement New-RadarChart cmdlet and associated classes

### Changed

- Update module v0.3.3
- Update SkiaSharp .NET dependency to v3.119.4
- Update HarfBuzzSharp .NET dependency to v8.3.1.5

## [0.3.2] - 2026-05-05

### Added

- Add New-SingleStackedBarChart cmdlet and update module exports
- Add pester test to validate the functionality of the New-SingleStackedBarChart cmdlet

### Changed

- Update module version to 0.3.2

## [0.3.1] - 2026-04-23

### Changed

- Update module version to 0.3.1
- Update ScottPlot .NET dependency to version v5.1.58

## [0.3.0] - 2026-03-14

### Added

- Add Signal chart support
- Add support for setting the background color of the chart
- Add documentation example on how to use the modules

### Changed

- Update module version to 0.3.0
- Restored the PowerShell module publishing workflow and added Twitter and Bluesky posting steps.


### Fixed

- Fix issue with the stacked chart not rendering correctly when using certain data sets
- Fix pester test to properly validate the functionality of the module

## [0.2.0] - 2026-02-20

### Added

- Add pester test to validate the functionality of the module

### Changed

- Update module version to 0.2.0

## [0.1.0] - 2026-02-19

### Added


- Initial release of AsBuiltReport Chart, providing basic charting capabilities for AsBuiltReport data visualization


