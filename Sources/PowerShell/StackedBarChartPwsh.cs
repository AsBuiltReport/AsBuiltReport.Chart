using System.Management.Automation;

namespace AsBuiltReportChart.PowerShell
{
    [Cmdlet(VerbsCommon.New, "StackedBar")]
    public class NewStackedBarChartCommand : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(
            Mandatory = false,
            HelpMessage = "Provide a filename for the chart. If not provided, a random filename will be generated."
            )]
        public string Filename { get; set; } = Chart.GenerateToken(8);

        [Parameter(Mandatory = true, HelpMessage = "Provide a list of double arrays for the values to be plotted. Each array represents a series in the stacked bar chart.")]
        public List<double[]>? Values { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Provide an array of strings for the labels of each bar in the chart.")]
        public string[]? Labels { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Povide an array of strings for the legend categories in the chart.")]
        public string[]? LegendCategories { get; set; }

        // Title settings
        [Parameter(Mandatory = true, HelpMessage = "Provide a title for the chart. If not provided, no title will be displayed.")]
        public string? Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Enable bold font for the chart title.")]
        public SwitchParameter TitleFontBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the font size for the chart title.")]
        public int TitleFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Set the font color for the chart title.")]
        public BasicColors TitleFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Enable the legend for the chart.")]
        public SwitchParameter EnableLegend { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the orientation of the legend.")]
        public Enums.Orientations LegendOrientation { get; set; } = Enums.Orientations.Vertical;

        [Parameter(Mandatory = false, HelpMessage = "Set the alignment of the legend.")]
        public Enums.Alignments LegendAlignment { get; set; } = Enums.Alignments.UpperRight;

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
        public string[]? CustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Set the font name for the chart.")]
        public string FontName { get; set; } = "Arial";

        // Label Font settings
        [Parameter(Mandatory = false, HelpMessage = "Set the font size for the chart labels.")]
        public int LabelFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Set the font color for the chart labels.")]
        public BasicColors LabelFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Enable bold font for the chart labels.")]
        public SwitchParameter LabelBold { get; set; }

        // Set font for the X and Y axis labels (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Set the label for the Y-axis.")]
        public string LabelYAxis { get; set; } = "Count";

        [Parameter(Mandatory = false, HelpMessage = "Set the label for the X-axis.")]
        public string LabelXAxis { get; set; } = "Values";

        // this set the orientation chart area  (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Set the orientation of the chart area.")]
        public Enums.Orientations AreaOrientation { get; set; } = Enums.Orientations.Vertical;

        // this set the area axes margins  (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Set the top margin for the chart area axes.")]
        public double AxesMarginsTop { get; set; } = 0.2;

        [Parameter(Mandatory = false, HelpMessage = "Set the bottom margin for the chart area axes.")]
        public double AxesMarginsDown { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Set the left margin for the chart area axes.")]
        public double AxesMarginsLeft { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Set the right margin for the chart area axes.")]
        public double AxesMarginsRight { get; set; } = 0.05;

        // Set chart Size WxH
        [Parameter(Mandatory = false, HelpMessage = "Set the width of the chart in pixels.")]
        public int Width { get; set; } = 400;

        [Parameter(Mandatory = false, HelpMessage = "Set the height of the chart in pixels.")]
        public int Height { get; set; } = 300;

        // Set OutputFolderPath
        [Parameter(Mandatory = false, HelpMessage = "Set the output folder path for the chart file.")]
        public string OutputFolderPath { get; set; } = Directory.GetCurrentDirectory();

        protected override void ProcessRecord()
        {
            if (Values != null && Labels != null && LegendCategories != null)
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
                StackedBar myStackedBar = new();
                WriteObject(myStackedBar.Chart(Values, Labels, LegendCategories, Filename, Width, Height));
            }
            else
            {
                WriteObject("Please provide Values, Labels and  LegendCategories parameters.");
            }
        }
    }
}