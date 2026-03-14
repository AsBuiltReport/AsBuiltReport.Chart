using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AsBuiltReportChart.Enums;

namespace AsBuiltReportChart
{
    internal partial class Chart
    {
        // Save settings (All Charts)
        internal static Formats Format { get; set; } = Formats.png;

        // Title setting  (All Charts)
        public static string Title { get; set; }
        public static bool TitleFontBold { get; set; }
        public static int TitleFontSize { get; set; } = 14;
        public static BasicColors TitleFontColor { get; set; } = BasicColors.Black;

        // Global Font settings  (All Charts)
        public static string FontName { get; set; } = "Arial";

        // Label Font settings  (All Charts)
        public static int LabelFontSize { get; set; } = 14;
        public static BasicColors LabelFontColor { get; set; } = BasicColors.Black;
        public static bool LabelBold { get; set; }

        // Set font for the X and Y axis labels (Bar Chart)
        public static string LabelYAxis { get; set; } = "Count";
        public static string LabelXAxis { get; set; } = "Values";

        // this set the distance of the labels from the chart center (Pie Chart)
        internal static double _labelDistance = 0.6;
        public static double LabelDistance
        {
            get { return _labelDistance; }
            set
            {
                if (value >= 0.5 && value <= 0.9)
                {
                    _labelDistance = value;
                }
                else
                {
                    throw new ArgumentException("Error: LabelDistance value range must be from 0.5 to 0.9.");
                }
            }
        }

        // this set the orientation chart area  (Bar Chart)
        public static Orientations AreaOrientation { get; set; } = Orientations.Vertical;

        // this set the distance of the chart area elements (Pie Chart)
        internal static double _areaExplodeFraction;
        public static double AreaExplodeFraction
        {
            get { return _areaExplodeFraction; }
            set
            {
                if (value >= 0.0 && value <= 0.5)
                {
                    _areaExplodeFraction = value;
                }
                else
                {
                    throw new ArgumentException("Error: AreaExplodeFraction value range must be from 0.0 to 0.5.");
                }
            }
        }

        // Legend setting (Pie Chart)
        public static bool EnableLegend { get; set; }

        // Legend Font settings (Pie Chart)
        public static int LegendFontSize { get; set; } = 12;
        public static BasicColors LegendFontColor { get; set; } = BasicColors.Black;
        public static bool LegendBold { get; set; }

        // Legend border settings (Pie Chart)
        public static BorderStyles LegendBorderStyle { get; set; } = BorderStyles.Solid;
        public static int LegendBorderSize { get; set; } = 1;
        public static BasicColors LegendBorderColor { get; set; } = BasicColors.Black; // Todo change this to rgb color
        public static Orientations LegendOrientation { get; set; } = Orientations.Vertical;

        public static Alignments LegendAlignment { get; set; } = Alignments.LowerRight;

        // Chart border settings (All Charts)
        public static bool EnableChartBorder { get; set; }
        public static BorderStyles ChartBorderStyle { get; set; }
        public static int ChartBorderSize { get; set; } = 1;
        public static BasicColors ChartBorderColor { get; set; } = BasicColors.Black;  // Todo change this to rgb color

        // Color Palette settings (All Charts)
        internal static readonly IReadOnlyDictionary<ColorPalettes, IPalette> ColorPaletteMap = new Dictionary<ColorPalettes, IPalette>()
    {
            { ColorPalettes.Amber, new ScottPlot.Palettes.Amber() },
            { ColorPalettes.Category10, new ScottPlot.Palettes.Category10() },
            { ColorPalettes.Category20, new ScottPlot.Palettes.Category20() },
            { ColorPalettes.Aurora, new ScottPlot.Palettes.Aurora() },
            { ColorPalettes.Building, new ScottPlot.Palettes.Building() },
            { ColorPalettes.ColorblindFriendly, new ScottPlot.Palettes.ColorblindFriendly() },
            { ColorPalettes.ColorblindFriendlyDark, new ScottPlot.Palettes.ColorblindFriendlyDark() },
            { ColorPalettes.Dark, new ScottPlot.Palettes.Dark() },
            { ColorPalettes.DarkPastel, new ScottPlot.Palettes.DarkPastel() },
            { ColorPalettes.Frost, new ScottPlot.Palettes.Frost() },
            { ColorPalettes.LightOcean, new ScottPlot.Palettes.LightOcean() },
            { ColorPalettes.LightSpectrum, new ScottPlot.Palettes.LightSpectrum() },
            { ColorPalettes.Microcharts, new ScottPlot.Palettes.Microcharts() },
            { ColorPalettes.Nero, new ScottPlot.Palettes.Nero() },
            { ColorPalettes.Nord, new ScottPlot.Palettes.Nord() },
            { ColorPalettes.Normal, new ScottPlot.Palettes.Normal() },
            { ColorPalettes.OneHalf, new ScottPlot.Palettes.OneHalf() },
            { ColorPalettes.OneHalfDark, new ScottPlot.Palettes.OneHalfDark() },
            { ColorPalettes.PastelWheel, new ScottPlot.Palettes.PastelWheel() },
            { ColorPalettes.Penumbra, new ScottPlot.Palettes.Penumbra() },
            { ColorPalettes.PolarNight, new ScottPlot.Palettes.PolarNight() },
            { ColorPalettes.Redness, new ScottPlot.Palettes.Redness() },
            { ColorPalettes.SnowStorm, new ScottPlot.Palettes.SnowStorm() },
            { ColorPalettes.SummerSplash, new ScottPlot.Palettes.SummerSplash() },
            { ColorPalettes.Tsitsulin, new ScottPlot.Palettes.Tsitsulin() },
    };
        internal static IPalette colorPalette;
        public static ColorPalettes? ColorPalette
        {
            get => ColorPaletteMap.FirstOrDefault(x => x.Value == colorPalette).Key;
            set
            {
                if (value != null)
                {
                    colorPalette = ColorPaletteMap[value.Value];
                }
            }
        }

        // Custom color palette (All Charts)
        public static bool InvertCustomColorPalette;
        internal static string[] _customColorPalette;
        public static string[] CustomColorPalette
        {
            get => _customColorPalette ?? Array.Empty<string>();
            set
            {
                if (value != null && value.Length > 0)
                {
                    foreach (var color in value)
                    {
                        if (!IsValidHexColor(color))
                        {
                            throw new ArgumentException($"Error: '{color}' is not a valid hex color code.");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Error: CustomColorPalette cannot be null or empty when setting custom colors.");
                }
                if (InvertCustomColorPalette)
                {
                    Array.Reverse(value);
                    _customColorPalette = value;
                }
                else
                {
                    _customColorPalette = value;
                }
            }
        }
        public static bool EnableCustomColorPalette { get; set; }

        internal static string _outputFolderPath;

        public static string OutputFolderPath
        {
            get
            {
                return _outputFolderPath ?? Directory.GetCurrentDirectory();
            }
            set
            {
                if (value is null)
                {
                    _outputFolderPath = Directory.GetCurrentDirectory();
                }
                else if (Directory.Exists(value))
                {
                    _outputFolderPath = value;
                }
                else
                {
                    throw new ArgumentException("Error: Directory Not Found Exception");
                }
            }
        }

        public static bool IsValidHexColor(string hexCode)
        {
            // Regex for #RGB, #RRGGBB, #RGBA, or #RRGGBBAA formats (case-insensitive)
            return HexColorRegex.IsMatch(hexCode);
        }

        private static readonly Regex HexColorRegex = new Regex("^#([A-Fa-f0-9]{3,4}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$", RegexOptions.Compiled);

        internal static readonly IReadOnlyDictionary<BorderStyles, LinePattern> LegendBorderStyleMap = new Dictionary<BorderStyles, LinePattern>()
    {
        {BorderStyles.Solid, LinePattern.Solid},
        {BorderStyles.Dashed , LinePattern.Dashed},
        {BorderStyles.Dotted ,LinePattern.Dotted},
        {BorderStyles.DenselyDashed, LinePattern.DenselyDashed},
    };

        internal static readonly IReadOnlyDictionary<Orientations, Orientation> LegendOrientationMap = new Dictionary<Orientations, Orientation>()
    {
        {Orientations.Horizontal, Orientation.Horizontal},
        {Orientations.Vertical, Orientation.Vertical},
    };

        internal static readonly IReadOnlyDictionary<Alignments, Alignment> LegendAlignmentMap = new Dictionary<Alignments, Alignment>()
    {
        {Alignments.LowerCenter, Alignment.LowerCenter},
        {Alignments.LowerLeft,Alignment.LowerLeft},
        {Alignments.LowerRight, Alignment.LowerRight},
        {Alignments.MiddleCenter,Alignment.MiddleCenter},
        {Alignments.MiddleLeft, Alignment.MiddleLeft},
        {Alignments.MiddleRight,Alignment.MiddleRight},
        {Alignments.UpperCenter,Alignment.UpperCenter},
        {Alignments.UpperLeft,Alignment.UpperLeft},
        {Alignments.UpperRight, Alignment.UpperRight},
    };
        internal static readonly IReadOnlyDictionary<BorderStyles, LinePattern> ChartBorderStyleMap = new Dictionary<BorderStyles, LinePattern>()
    {
        {BorderStyles.Solid, LinePattern.Solid},
        {BorderStyles.Dashed, LinePattern.Dashed},
        {BorderStyles.Dotted, LinePattern.Dotted},
        {BorderStyles.DenselyDashed, LinePattern.DenselyDashed},
    };

        internal static readonly IReadOnlyDictionary<BasicColors, Color> ColorMap = new Dictionary<BasicColors, Color>()
    {
        { BasicColors.Black,  Colors.Black },
        { BasicColors.White,  Colors.White },
        { BasicColors.Red,  Colors.Red },
        { BasicColors.Yellow,  Colors.Yellow },
        { BasicColors.Green,  Colors.Green },
        { BasicColors.Brown,  Colors.Brown },
        { BasicColors.Orange,  Colors.Orange },
        { BasicColors.Pink,  Colors.Pink },
        { BasicColors.Purple,  Colors.Purple },
        { BasicColors.Gray,  Colors.Gray },
        { BasicColors.Blue,  Colors.Blue },
        { BasicColors.DarkBlue,  Colors.DarkBlue },
        { BasicColors.DarkGreen,  Colors.DarkGreen },
    };

        internal static readonly IReadOnlyDictionary<Alignments, Alignment> WatermarkAlignmentMap = new Dictionary<Alignments, Alignment>()
    {
        {Alignments.LowerCenter, Alignment.LowerCenter},
        {Alignments.LowerLeft,Alignment.LowerLeft},
        {Alignments.LowerRight, Alignment.LowerRight},
        {Alignments.MiddleCenter,Alignment.MiddleCenter},
        {Alignments.MiddleLeft, Alignment.MiddleLeft},
        {Alignments.MiddleRight,Alignment.MiddleRight},
        {Alignments.UpperCenter,Alignment.UpperCenter},
        {Alignments.UpperLeft,Alignment.UpperLeft},
        {Alignments.UpperRight, Alignment.UpperRight},
    };

        // Set area axes margins
        internal static double _axesMarginsTop = 0.07;
        public static double AxesMarginsTop
        {
            get { return _axesMarginsTop; }
            set
            {
                if (value >= 0.0 && value <= 1)
                {
                    _axesMarginsTop = value;
                }
                else
                {
                    throw new ArgumentException("Error: AxesMarginsTop value range must be from 0.0 to 1.0 (fractions).");
                }
            }
        }
        internal static double _axesMarginsDown = 0.07;
        public static double AxesMarginsDown
        {
            get { return _axesMarginsDown; }
            set
            {
                if (value >= 0.0 && value <= 1)
                {
                    _axesMarginsDown = value;
                }
                else
                {
                    throw new ArgumentException("Error: AxesMarginsDown value range must be from 0.0 to 1.0 (fractions).");
                }
            }
        }
        internal static double _axesMarginsLeft = 0.05;
        public static double AxesMarginsLeft
        {
            get { return _axesMarginsLeft; }
            set
            {
                if (value >= 0.0 && value <= 1)
                {
                    _axesMarginsLeft = value;
                }
                else
                {
                    throw new ArgumentException("Error: AxesMarginsLeft value range must be from 0.0 to 1.0 (fractions).");
                }
            }
        }
        internal static double _axesMarginsRight = 0.05;
        public static double AxesMarginsRight
        {
            get { return _axesMarginsRight; }
            set
            {
                if (value >= 0.0 && value <= 1)
                {
                    _axesMarginsRight = value;
                }
                else
                {
                    throw new ArgumentException("Error: AxesMarginsRight value range must be from 0.0 to 1.0 (fractions).");
                }
            }
        }

        // Chart background color settings (All Charts)
        public static BasicColors? FigureBackgroundColor { get; set; }
        public static BasicColors? DataBackgroundColor { get; set; }

        // Watermark settings (All Charts)
        public static bool EnableWatermark { get; set; }
        public static string WatermarkText { get; set; } = "Confidential";

        public static Alignments WatermarkAlignment { get; set; } = Alignments.MiddleCenter;

        public static float WatermarkRotation { get; set; } = 0;

        public static string WatermarkFontName { get; set; } = "Arial";
        public static int WatermarkFontSize { get; set; } = 24;
        public static BasicColors WatermarkColor { get; set; } = BasicColors.Gray;

        internal static double _watermarkOpacity = 0.3;
        public static double WatermarkOpacity
        {
            get { return _watermarkOpacity; }
            set
            {
                if (value >= 0.0 && value <= 1.0)
                {
                    _watermarkOpacity = value;
                }
                else
                {
                    throw new ArgumentException("Error: WatermarkOpacity value range must be from 0.0 to 1.0.");
                }
            }
        }

        internal static void ApplyWatermark(Plot plot)
        {
            if (!EnableWatermark || string.IsNullOrEmpty(WatermarkText))
                return;

            var annotation = plot.Add.Annotation(WatermarkText, WatermarkAlignmentMap[WatermarkAlignment]);
            annotation.LabelFontColor = ColorMap[WatermarkColor].WithOpacity(WatermarkOpacity);
            annotation.LabelFontSize = WatermarkFontSize;
            annotation.LabelFontName = WatermarkFontName;
            annotation.LabelBackgroundColor = Colors.Transparent;
            annotation.LabelBorderColor = Colors.Transparent;
            annotation.LabelBorderWidth = 0;
            annotation.LabelShadowColor = Colors.Transparent;
            annotation.LabelRotation = WatermarkRotation;
        }

        public static Color GetDrawingColor(BasicColors color)
        {
            return ColorMap[color];
        }

        internal static void Reset()
        {
            Format = Formats.png;
            Title = null;
            TitleFontBold = false;
            TitleFontSize = 14;
            TitleFontColor = BasicColors.Black;
            FontName = "Arial";
            LabelFontSize = 14;
            LabelFontColor = BasicColors.Black;
            LabelBold = false;
            LabelYAxis = "Count";
            LabelXAxis = "Values";
            _labelDistance = 0.6;
            AreaOrientation = Orientations.Vertical;
            _areaExplodeFraction = 0;
            EnableLegend = false;
            LegendFontSize = 12;
            LegendFontColor = BasicColors.Black;
            LegendBold = false;
            LegendBorderStyle = BorderStyles.Solid;
            LegendBorderSize = 1;
            LegendBorderColor = BasicColors.Black;
            LegendOrientation = Orientations.Vertical;
            LegendAlignment = Alignments.LowerRight;
            EnableChartBorder = false;
            ChartBorderStyle = BorderStyles.Solid;
            ChartBorderSize = 1;
            ChartBorderColor = BasicColors.Black;
            colorPalette = ColorPaletteMap[ColorPalettes.Category10];
            InvertCustomColorPalette = false;
            _customColorPalette = null;
            EnableCustomColorPalette = false;
            _outputFolderPath = null;
            _axesMarginsTop = 0.07;
            _axesMarginsDown = 0.07;
            _axesMarginsLeft = 0.05;
            _axesMarginsRight = 0.05;
            FigureBackgroundColor = null;
            DataBackgroundColor = null;
            EnableWatermark = false;
            WatermarkText = "Confidential";
            WatermarkAlignment = Alignments.MiddleCenter;
            WatermarkRotation = 0;
            WatermarkFontName = "Arial";
            WatermarkFontSize = 24;
            WatermarkColor = BasicColors.Gray;
            _watermarkOpacity = 0.3;
        }

        public static string GenerateToken(Byte length)
        {
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes).Replace("=", "").Replace("+", "").Replace("/", "");
        }
        public static object SaveInFormat(Plot plot, int width, int height, string filepath, string filename, Formats Format)
        {
            switch (Format)
            {
                case Formats.png:
                    plot.SavePng(Path.Combine(filepath, $"{filename}.png"), width, height);
                    if (File.Exists(Path.Combine(filepath, $"{filename}.png")))
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(filepath, $"{filename}.png"));
                        return fileInfo;
                    }
                    else
                    {
                        throw new ArgumentException("Error: Unable to Export Chart Exception");
                    }
                case Formats.jpg:
                    plot.SaveJpeg(Path.Combine(filepath, $"{filename}.jpg"), width, height);
                    if (File.Exists(Path.Combine(filepath, $"{filename}.jpg")))
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(filepath, $"{filename}.jpg"));
                        return fileInfo;
                    }
                    else
                    {
                        throw new ArgumentException("Error: Unable to Export Chart Exception");
                    }
                case Formats.jpeg:
                    plot.SaveJpeg(Path.Combine(filepath, $"{filename}.jpeg"), width, height);
                    if (File.Exists(Path.Combine(filepath, $"{filename}.jpeg")))
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(filepath, $"{filename}.jpeg"));
                        return fileInfo;
                    }
                    else
                    {
                        throw new ArgumentException("Error: Unable to Export Chart Exception");
                    }
                case Formats.bmp:
                    plot.SaveBmp(Path.Combine(filepath, $"{filename}.bmp"), width, height);
                    if (File.Exists(Path.Combine(filepath, $"{filename}.bmp")))
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(filepath, $"{filename}.bmp"));
                        return fileInfo;
                    }
                    else
                    {
                        throw new ArgumentException("Error: Unable to Export Chart Exception");
                    }
                case Formats.svg:
                    plot.SaveSvg(Path.Combine(filepath, $"{filename}.svg"), width, height);
                    if (File.Exists(Path.Combine(filepath, $"{filename}.svg")))
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(filepath, $"{filename}.svg"));
                        return fileInfo;
                    }
                    else
                    {
                        throw new ArgumentException("Error: Unable to Export Chart Exception");
                    }
                case Formats.base64:
                    byte[] imgBytes = plot.GetImageBytes(width, height, ImageFormat.Png);
                    if (imgBytes != null)
                    {
                        return Convert.ToBase64String(imgBytes);
                    }
                    else
                    {
                        throw new ArgumentException("Error: Unable to Export Chart Exception");
                    }
                default:
                    throw new ArgumentException($"Error: Unsupported format '{Format}'.");
            }
        }
    }
}
