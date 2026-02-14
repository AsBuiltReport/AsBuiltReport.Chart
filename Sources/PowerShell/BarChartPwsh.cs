using System.Management.Automation;

namespace AsBuiltReportChart.PowerShell
{
    [Cmdlet(VerbsCommon.New, "BarChart")]
    public class NewBarChartCommand : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(Mandatory = false, HelpMessage = "Output filename for the chart. If not specified, a random token will be generated.")]
        public string Filename { get; set; } = Chart.GenerateToken(8);

        [Parameter(Mandatory = true, HelpMessage = "Array of numeric values to display in the bar chart.")]
        public double[]? Values { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Array of labels for each bar in the chart.")]
        public string[]? Labels { get; set; }

        // Title settings
        [Parameter(Mandatory = true, HelpMessage = "Title text for the chart.")]
        public string? Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Make the title font bold.")]
        public SwitchParameter TitleFontBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font size for the title in points.")]
        public int TitleFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for the title.")]
        public BasicColors TitleFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Enable the legend on the chart.")]
        public SwitchParameter EnableLegend { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Orientation of the legend (Horizontal or Vertical).")]
        public Enums.Orientations LegendOrientation { get; set; } = Enums.Orientations.Vertical;

        [Parameter(Mandatory = false, HelpMessage = "Alignment position of the legend on the chart.")]
        public Enums.Alignments LegendAlignment { get; set; } = Enums.Alignments.UpperRight;

        [Parameter(Mandatory = false, HelpMessage = "Font size for the legend text in points.")]
        public int LegendFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Make the legend font bold.")]
        public SwitchParameter LegendlBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font color for the legend text.")]
        public BasicColors LegendFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Border style for the legend box.")]
        public Enums.BorderStyles LegendBorderStyle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Border size for the legend box in pixels.")]
        public int LegendBorderSize { get; set; } = 1;

        [Parameter(Mandatory = false, HelpMessage = "Border color for the legend box.")]
        public BasicColors LegendBorderColor { get; set; } = BasicColors.Black;

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

        [Parameter(Mandatory = false, HelpMessage = "Color palette preset to use for the chart.")]
        public Enums.ColorPalettes ColorPalette { get; set; } = Enums.ColorPalettes.Category10;

        [Parameter(Mandatory = false, HelpMessage = "Enable custom color palette for the chart.")]
        public SwitchParameter EnableCustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Invert the custom color palette.")]
        public SwitchParameter InvertCustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Array of custom hex color codes for the chart bars.")]
        public string[]? CustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font name to use for all text in the chart.")]
        public string FontName { get; set; } = "Arial";

        // Label Font settings
        [Parameter(Mandatory = false, HelpMessage = "Font size for axis labels in points.")]
        public int LabelFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for axis labels.")]
        public BasicColors LabelFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Make axis label fonts bold.")]
        public SwitchParameter LabelBold { get; set; }

        // Set font for the X and Y axis labels (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Label text for the Y-axis.")]
        public string LabelYAxis { get; set; } = "Count";

        [Parameter(Mandatory = false, HelpMessage = "Label text for the X-axis.")]
        public string LabelXAxis { get; set; } = "Values";

        // this set the orientation chart area  (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Orientation of the chart area (Horizontal or Vertical).")]
        public Enums.Orientations AreaOrientation { get; set; } = Enums.Orientations.Vertical;

        // this set the area axes margins  (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Top margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsTop { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Bottom margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsDown { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Left margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsLeft { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Right margin for the chart area as a percentage (0-1).")]
        public double AxesMarginsRight { get; set; } = 0.05;

        // Set chart Size WxH
        [Parameter(Mandatory = false, HelpMessage = "Width of the output chart in pixels. Defaults to 400.")]
        public int Width { get; set; } = 400;

        [Parameter(Mandatory = false, HelpMessage = "Height of the output chart in pixels. Defaults to 300.")]
        public int Height { get; set; } = 300;

        // Set OutputFolderPath
        [Parameter(Mandatory = false, HelpMessage = "Output folder path where the chart will be saved.")]
        public string OutputFolderPath { get; set; } = Directory.GetCurrentDirectory();

        protected override void ProcessRecord()
        {
            if (Values != null && Labels != null)
            {

                if (EnableLegend)
                {
                    Chart.EnableLegend = EnableLegend;
                    // Legend box settings
                    Chart.LegendOrientation = LegendOrientation;
                    Chart.LegendAlignment = LegendAlignment;

                    // Legend font settings
                    Chart.LegendFontSize = LegendFontSize;
                    Chart.LegendFontColor = LegendFontColor;
                    Chart.LegendlBold = LegendlBold;
                    // Legend border settings
                    Chart.LegendBorderStyle = LegendBorderStyle;
                    Chart.LegendBorderSize = LegendBorderSize;
                    Chart.LegendBorderColor = LegendBorderColor;
                }

                if (EnableChartBorder)
                {
                    // Chart area settings
                    Chart.EnableChartBorder = EnableChartBorder;
                    Chart.ChartBorderColor = ChartBorderColor;
                    Chart.ChartBorderSize = ChartBorderSize;
                    Chart.ChartBorderStyle = ChartBorderStyle;
                }
                // Color palette settings
                if (EnableCustomColorPalette)
                {
                    if (CustomColorPalette is not null or [])
                    {
                        // Set ScottPlot custom color palette
                        Chart.EnableCustomColorPalette = EnableCustomColorPalette;
                        Chart.InvertCustomColorPalette = InvertCustomColorPalette;
                        Chart.CustomColorPalette = CustomColorPalette;
                    }
                    else
                    {
                        throw new Exception("EnableCustomColorPalette requires CustomColorPalette to be set.");
                    }
                }
                else
                {
                    Chart.ColorPalette = ColorPalette;
                }

                // Title settings
                if (Title != null)
                {
                    Chart.Title = Title;
                    Chart.TitleFontBold = TitleFontBold;
                    Chart.TitleFontSize = TitleFontSize;
                    Chart.TitleFontColor = TitleFontColor;
                }

                // Font Settings
                Chart.FontName = FontName;
                Chart.LabelFontSize = LabelFontSize;
                Chart.LabelFontColor = LabelFontColor;
                Chart.LabelBold = LabelBold;

                // Set font for the X and Y axis labels
                Chart.LabelXAxis = LabelXAxis;
                Chart.LabelYAxis = LabelYAxis;

                // this set the orientation chart area  (Bar Chart)
                Chart.AreaOrientation = AreaOrientation;

                // this set the area axes margins  (Bar Chart)
                Chart.AxesMarginsTop = AxesMarginsTop;
                Chart.AxesMarginsDown = AxesMarginsDown;
                Chart.AxesMarginsLeft = AxesMarginsLeft;
                Chart.AxesMarginsRight = AxesMarginsRight;

                // Set file directory save path 
                Chart.OutputFolderPath = OutputFolderPath;

                Chart.Format = Format;
                Bar myBar = new();
                WriteObject(myBar.Chart(Values, Labels, Filename, Width, Height));
            }
            else
            {
                WriteObject("Please provide both Values and Labels parameters.");
            }
        }
    }
}