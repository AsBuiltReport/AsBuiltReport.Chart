using System;
using System.IO;
using System.Management.Automation;

namespace AsBuiltReportChart.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SingleStackedBarChart")]
    public class NewSingleStackedBarChartCommand : Cmdlet
    {
        [Parameter(
            Mandatory = false,
            HelpMessage = "Provide a filename for the chart. If not provided, a random filename will be generated."
            )]
        public string Filename { get; set; } = Chart.GenerateToken(8);

        [Parameter(Mandatory = true, HelpMessage = "Provide an array of doubles representing the percentage value of each segment in the single stacked bar.")]
        public double[] Values { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Provide a string for the label of the single bar on the category axis.")]
        public string Label { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Provide an array of strings for the legend categories, one per segment.")]
        public string[] LegendCategories { get; set; }

        // Title settings
        [Parameter(Mandatory = true, HelpMessage = "Provide a title for the chart.")]
        public string Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Enable bold font for the chart title.")]
        public SwitchParameter TitleFontBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the font size for the chart title.")]
        public int TitleFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Set the font color for the chart title.")]
        public BasicColors TitleFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Enable the legend for the chart.")]
        public SwitchParameter EnableLegend { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the orientation of the legend.")]
        public Enums.Orientations LegendOrientation { get; set; } = Enums.Orientations.Horizontal;

        [Parameter(Mandatory = false, HelpMessage = "Set the alignment of the legend.")]
        public Enums.Alignments LegendAlignment { get; set; } = Enums.Alignments.LowerCenter;

        [Parameter(Mandatory = false, HelpMessage = "Set the font size for the legend.")]
        public int LegendFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Set the font color for the legend.")]
        public BasicColors LegendFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Set the border style for the legend.")]
        public Enums.BorderStyles LegendBorderStyle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the border size for the legend.")]
        public int LegendBorderSize { get; set; } = 1;

        [Parameter(Mandatory = false, HelpMessage = "Set the border color for the legend.")]
        public BasicColors LegendBorderColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = true, HelpMessage = "Provide a format for the chart output.")]
        public Formats Format { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the border color for the chart area.")]
        public BasicColors ChartBorderColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Set the border size for the chart area.")]
        public int ChartBorderSize { get; set; } = 1;

        [Parameter(Mandatory = false, HelpMessage = "Set the border style for the chart area.")]
        public Enums.BorderStyles ChartBorderStyle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Enable the border for the chart area.")]
        public SwitchParameter EnableChartBorder { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the color palette for the chart.")]
        public Enums.ColorPalettes ColorPalette { get; set; } = Enums.ColorPalettes.Category10;

        [Parameter(Mandatory = false, HelpMessage = "Enable custom color palette for the chart.")]
        public SwitchParameter EnableCustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Invert the custom color palette.")]
        public SwitchParameter InvertCustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the custom color palette for the chart.")]
        public string[] CustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the font name for the chart.")]
        public string FontName { get; set; } = "Arial";

        // Label Font settings
        [Parameter(Mandatory = false, HelpMessage = "Set the font size for the chart labels.")]
        public int LabelFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Set the font color for the chart labels.")]
        public BasicColors LabelFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Enable bold font for the chart labels.")]
        public SwitchParameter LabelBold { get; set; }

        // Axis label text
        [Parameter(Mandatory = false, HelpMessage = "Set the label for the Y-axis.")]
        public string LabelYAxis { get; set; } = "";

        [Parameter(Mandatory = false, HelpMessage = "Set the label for the X-axis.")]
        public string LabelXAxis { get; set; } = "";

        // Chart orientation
        [Parameter(Mandatory = false, HelpMessage = "Set the orientation of the chart area (Vertical or Horizontal).")]
        public Enums.Orientations AreaOrientation { get; set; } = Enums.Orientations.Vertical;

        // Value axis suffix
        [Parameter(Mandatory = false, HelpMessage = "Suffix appended to each value label and axis tick (e.g. '%', ' GB', ' TB'). Defaults to no suffix.")]
        public string ValueSuffix { get; set; } = "";

        // Axes margins
        [Parameter(Mandatory = false, HelpMessage = "Set the top margin for the chart area axes.")]
        public double AxesMarginsTop { get; set; } = 0.2;

        [Parameter(Mandatory = false, HelpMessage = "Set the bottom margin for the chart area axes.")]
        public double AxesMarginsDown { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Set the left margin for the chart area axes.")]
        public double AxesMarginsLeft { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Set the right margin for the chart area axes.")]
        public double AxesMarginsRight { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Background color of the entire figure (canvas).")]
        public BasicColors? FigureBackgroundColor { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Background color of the data area inside the axes.")]
        public BasicColors? DataBackgroundColor { get; set; }

        // Watermark settings
        [Parameter(Mandatory = false, HelpMessage = "Enable a watermark on the chart.")]
        public SwitchParameter EnableWatermark { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Text to display as the watermark. Defaults to 'Confidential'.")]
        public string WatermarkText { get; set; } = "Confidential";

        [Parameter(Mandatory = false, HelpMessage = "Alignment of the watermark text. Defaults to 'MiddleCenter'.")]
        public Enums.Alignments WatermarkAlignment { get; set; } = Enums.Alignments.MiddleCenter;

        [Parameter(Mandatory = false, HelpMessage = "Font name for the watermark text.")]
        public string WatermarkFontName { get; set; } = "Arial";

        [Parameter(Mandatory = false, HelpMessage = "Font size for the watermark text in points. Defaults to 24.")]
        public int WatermarkFontSize { get; set; } = 24;

        [Parameter(Mandatory = false, HelpMessage = "Color of the watermark text.")]
        public BasicColors WatermarkColor { get; set; } = BasicColors.Gray;

        [Parameter(Mandatory = false, HelpMessage = "Opacity of the watermark (0.0 fully transparent to 1.0 fully opaque). Defaults to 0.3.")]
        public double WatermarkOpacity { get; set; } = 0.3;

        [Parameter(Mandatory = false, HelpMessage = "Rotation angle of the watermark text in degrees. Defaults to 0.")]
        public float WatermarkRotation { get; set; } = 0;

        // Chart size
        [Parameter(Mandatory = false, HelpMessage = "Set the width of the chart in pixels.")]
        public int Width { get; set; } = 400;

        [Parameter(Mandatory = false, HelpMessage = "Set the height of the chart in pixels.")]
        public int Height { get; set; } = 300;

        // Output folder
        [Parameter(Mandatory = false, HelpMessage = "Set the output folder path for the chart file.")]
        [ValidatePath()]
        public string OutputFolderPath { get; set; } = Directory.GetCurrentDirectory();

        protected override void ProcessRecord()
        {
            Chart.Reset();
            if (Values != null && Label != null && LegendCategories != null)
            {
                if (EnableLegend)
                {
                    Chart.EnableLegend = EnableLegend;
                    Chart.LegendOrientation = LegendOrientation;
                    Chart.LegendAlignment = LegendAlignment;
                    Chart.LegendFontSize = LegendFontSize;
                    Chart.LegendFontColor = LegendFontColor;
                    Chart.LegendBorderStyle = LegendBorderStyle;
                    Chart.LegendBorderSize = LegendBorderSize;
                    Chart.LegendBorderColor = LegendBorderColor;
                }

                if (EnableChartBorder)
                {
                    Chart.EnableChartBorder = EnableChartBorder;
                    Chart.ChartBorderColor = ChartBorderColor;
                    Chart.ChartBorderSize = ChartBorderSize;
                    Chart.ChartBorderStyle = ChartBorderStyle;
                }

                if (EnableCustomColorPalette)
                {
                    if (CustomColorPalette != null && CustomColorPalette.Length > 0)
                    {
                        Chart.EnableCustomColorPalette = EnableCustomColorPalette;
                        Chart.InvertCustomColorPalette = InvertCustomColorPalette;
                        Chart.CustomColorPalette = CustomColorPalette;
                    }
                    else
                    {
                        throw new InvalidOperationException("EnableCustomColorPalette requires CustomColorPalette to be set.");
                    }
                }
                else
                {
                    Chart.ColorPalette = ColorPalette;
                }

                if (Title != null)
                {
                    Chart.Title = Title;
                    Chart.TitleFontBold = TitleFontBold;
                    Chart.TitleFontSize = TitleFontSize;
                    Chart.TitleFontColor = TitleFontColor;
                }

                Chart.FontName = FontName;
                Chart.LabelFontSize = LabelFontSize;
                Chart.LabelFontColor = LabelFontColor;
                Chart.LabelBold = LabelBold;

                Chart.LabelXAxis = LabelXAxis;
                Chart.LabelYAxis = LabelYAxis;

                Chart.AreaOrientation = AreaOrientation;
                Chart.ValueSuffix = ValueSuffix;

                Chart.AxesMarginsTop = AxesMarginsTop;
                Chart.AxesMarginsDown = AxesMarginsDown;
                Chart.AxesMarginsLeft = AxesMarginsLeft;
                Chart.AxesMarginsRight = AxesMarginsRight;

                Chart.FigureBackgroundColor = FigureBackgroundColor;
                Chart.DataBackgroundColor = DataBackgroundColor;

                Chart.EnableWatermark = EnableWatermark;
                Chart.WatermarkText = WatermarkText;
                Chart.WatermarkAlignment = WatermarkAlignment;
                Chart.WatermarkFontName = WatermarkFontName;
                Chart.WatermarkFontSize = WatermarkFontSize;
                Chart.WatermarkColor = WatermarkColor;
                Chart.WatermarkOpacity = WatermarkOpacity;
                Chart.WatermarkRotation = WatermarkRotation;

                Chart.OutputFolderPath = OutputFolderPath;
                Chart.Format = Format;

                SingleStackedBar mySingleStackedBar = new SingleStackedBar();
                WriteObject(mySingleStackedBar.Chart(Values, Label, LegendCategories, Filename, Width, Height));
            }
            else
            {
                WriteObject("Please provide Values, Label and LegendCategories parameters.");
            }
        }
    }
}
