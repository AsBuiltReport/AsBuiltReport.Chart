using System;
using System.IO;
using System.Management.Automation;

namespace AsBuiltReportChart.PowerShell
{
    [Cmdlet(VerbsCommon.New, "HeatMapChart")]
    public class NewHeatMapChartCommand : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Output filename for the chart. If not specified, a random token will be generated.")]
        public string Filename { get; set; } = Chart.GenerateToken(8);

        [Parameter(Mandatory = true, HelpMessage = "Matrix of numeric values to display in the heat map.")]
        public double[][] Values { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Row labels for the heat map.")]
        public string[] RowLabels { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Column labels for the heat map.")]
        public string[] ColumnLabels { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Title text for the chart.")]
        public string Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Make the title font bold.")]
        public SwitchParameter TitleFontBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font size for the title in points.")]
        public int TitleFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for the title.")]
        public BasicColors TitleFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Enable the heat map color scale legend.")]
        public SwitchParameter EnableLegend { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Output format for the chart (PNG, JPEG, etc.).")]
        public Formats Format { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Border color for the chart area.")]
        public BasicColors ChartBorderColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Border size for the chart area in pixels.")]
        public int ChartBorderSize { get; set; } = 1;

        [Parameter(Mandatory = false, HelpMessage = "Border style for the chart area.")]
        public Enums.BorderStyles ChartBorderStyle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Enable border around the chart area.")]
        public SwitchParameter EnableChartBorder { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font name to use for all text in the chart.")]
        public string FontName { get; set; } = "Arial";

        [Parameter(Mandatory = false, HelpMessage = "Font size for axis labels in points.")]
        public int LabelFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for axis labels.")]
        public BasicColors LabelFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Make axis label fonts bold.")]
        public SwitchParameter LabelBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Label text for the Y-axis.")]
        public string LabelYAxis { get; set; } = "Rows";

        [Parameter(Mandatory = false, HelpMessage = "Label text for the X-axis.")]
        public string LabelXAxis { get; set; } = "Columns";

        [Parameter(Mandatory = false, HelpMessage = "Top margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsTop { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Bottom margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsDown { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Left margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsLeft { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Right margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsRight { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Background color of the entire figure (canvas).")]
        public BasicColors? FigureBackgroundColor { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Background color of the data area inside the axes.")]
        public BasicColors? DataBackgroundColor { get; set; }

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

        [Parameter(Mandatory = false, HelpMessage = "Width of the output chart in pixels. Defaults to 400.")]
        public int Width { get; set; } = 400;

        [Parameter(Mandatory = false, HelpMessage = "Height of the output chart in pixels. Defaults to 300.")]
        public int Height { get; set; } = 300;

        [Parameter(Mandatory = false, HelpMessage = "Output folder path where the chart will be saved.")]
        [ValidatePath()]
        public string OutputFolderPath { get; set; } = Directory.GetCurrentDirectory();

        protected override void ProcessRecord()
        {
            Chart.Reset();

            if (Values == null)
            {
                throw new ArgumentException("Error: Values cannot be null.");
            }

            Chart.Title = Title;
            Chart.TitleFontBold = TitleFontBold;
            Chart.TitleFontSize = TitleFontSize;
            Chart.TitleFontColor = TitleFontColor;
            Chart.FontName = FontName;
            Chart.LabelFontSize = LabelFontSize;
            Chart.LabelFontColor = LabelFontColor;
            Chart.LabelBold = LabelBold;
            Chart.LabelXAxis = LabelXAxis;
            Chart.LabelYAxis = LabelYAxis;
            Chart.AxesMarginsTop = AxesMarginsTop;
            Chart.AxesMarginsDown = AxesMarginsDown;
            Chart.AxesMarginsLeft = AxesMarginsLeft;
            Chart.AxesMarginsRight = AxesMarginsRight;
            Chart.FigureBackgroundColor = FigureBackgroundColor;
            Chart.DataBackgroundColor = DataBackgroundColor;
            Chart.EnableLegend = EnableLegend;
            Chart.EnableChartBorder = EnableChartBorder;
            Chart.ChartBorderColor = ChartBorderColor;
            Chart.ChartBorderSize = ChartBorderSize;
            Chart.ChartBorderStyle = ChartBorderStyle;
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

            HeatMap heatMap = new HeatMap();
            WriteObject(heatMap.Chart(Values, RowLabels, ColumnLabels, Filename, Width, Height));
        }
    }
}
