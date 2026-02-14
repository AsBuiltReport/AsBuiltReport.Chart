using System.Management.Automation;

namespace AsBuiltReportChart.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PieChart")]
    public class NewPieChartCommand : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(Mandatory = false, HelpMessage = "Output filename for the chart. Defaults to a randomly generated 8-character token.")]
        public string Filename { get; set; } = Chart.GenerateToken(8);

        [Parameter(Mandatory = true, HelpMessage = "Array of numeric values to display in the pie chart.")]
        public double[]? Values { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Array of labels corresponding to each pie slice.")]
        public string[]? Labels { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Title text to display on the chart.")]
        public string? Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Switch to make the title font bold.")]
        public SwitchParameter TitleFontBold { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font size for the title. Defaults to 14.")]
        public int TitleFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for the title.")]
        public BasicColors TitleFontColor { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Switch to enable the legend on the chart.")]
        public SwitchParameter EnableLegend { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Orientation of the legend (Vertical or Horizontal). Defaults to Vertical.")]
        public Enums.Orientations LegendOrientation { get; set; } = Enums.Orientations.Vertical;

        [Parameter(Mandatory = false, HelpMessage = "Alignment of the legend on the chart. Defaults to UpperRight.")]
        public Enums.Alignments LegendAlignment { get; set; } = Enums.Alignments.UpperRight;

        [Parameter(Mandatory = true, HelpMessage = "Output format for the chart (e.g., PNG, JPG, SVG).")]
        public Formats Format { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Fraction to explode pie slices (0.0 to 0.5).")]
        [ValidateSet("0.0", "0.1", "0.2", "0.3", "0.4", "0.5")]
        public double AreaExplodeFraction { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Color of the chart border.")]
        public BasicColors ChartBorderColor { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Size of the chart border in pixels. Defaults to 1.")]
        public int ChartBorderSize { get; set; } = 1;

        [Parameter(Mandatory = false, HelpMessage = "Style of the chart border.")]
        public Enums.BorderStyles ChartBorderStyle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Switch to enable the chart border.")]
        public SwitchParameter EnableChartBorder { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font size for the legend. Defaults to 14.")]
        public int LegendFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for the legend. Defaults to Black.")]
        public BasicColors LegendFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Border style for the legend.")]
        public Enums.BorderStyles LegendBorderStyle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Border size for the legend in pixels. Defaults to 1.")]
        public int LegendBorderSize { get; set; } = 1;

        [Parameter(Mandatory = false, HelpMessage = "Border color for the legend. Defaults to Black.")]
        public BasicColors LegendBorderColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Color palette for the chart. Defaults to Category10.")]
        public Enums.ColorPalettes ColorPalette { get; set; } = Enums.ColorPalettes.Category10;

        [Parameter(Mandatory = false, HelpMessage = "Switch to enable custom color palette.")]
        public SwitchParameter EnableCustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Invert the custom color palette.")]
        public SwitchParameter InvertCustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Array of custom hex color values for the chart.")]
        public string[]? CustomColorPalette { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Font name to use for all text. Defaults to Arial.")]
        public string FontName { get; set; } = "Arial";

        // Label Font settings
        [Parameter(Mandatory = false, HelpMessage = "Font size for chart labels. Defaults to 14.")]
        public int LabelFontSize { get; set; } = 14;

        [Parameter(Mandatory = false, HelpMessage = "Font color for labels. Defaults to Black.")]
        public BasicColors LabelFontColor { get; set; } = BasicColors.Black;

        [Parameter(Mandatory = false, HelpMessage = "Switch to make label font bold.")]
        public SwitchParameter LabelBold { get; set; }

        // this set the distance of the labels from the chart center (Pie Chart)
        [Parameter(Mandatory = false, HelpMessage = "Distance of labels from the chart center (0.5 to 0.9). Defaults to 0.6.")]
        [ValidateSet("0.5", "0.6", "0.7", "0.8", "0.9")]
        public double LabelDistance { get; set; } = 0.6;

        // this set the area axes margins  (Bar Chart)
        [Parameter(Mandatory = false, HelpMessage = "Top margin for the chart area. Defaults to 0.2.")]
        public double AxesMarginsTop { get; set; } = 0.2;

        [Parameter(Mandatory = false, HelpMessage = "Bottom margin for the chart area. Defaults to 0.05.")]
        public double AxesMarginsDown { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Left margin for the chart area. Defaults to 0.05.")]
        public double AxesMarginsLeft { get; set; } = 0.05;

        [Parameter(Mandatory = false, HelpMessage = "Right margin for the chart area. Defaults to 0.05.")]
        public double AxesMarginsRight { get; set; } = 0.05;

        // Set chart Size WxH
        [Parameter(Mandatory = false, HelpMessage = "Width of the chart in pixels. Defaults to 400.")]
        public int Width { get; set; } = 400;

        [Parameter(Mandatory = false, HelpMessage = "Height of the chart in pixels. Defaults to 300.")]
        public int Height { get; set; } = 300;

        // Set OutputFolderPath
        [Parameter(Mandatory = false, HelpMessage = "Directory path where the chart file will be saved. Defaults to current directory.")]
        [ValidatePath()]
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
                    // Legend border settings
                    Chart.LegendBorderStyle = LegendBorderStyle;
                    Chart.LegendBorderSize = LegendBorderSize;
                    Chart.LegendBorderColor = LegendBorderColor;
                }

                Chart.AreaExplodeFraction = AreaExplodeFraction;
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

                // this set the area axes margins  (Bar Chart)
                Chart.AxesMarginsTop = AxesMarginsTop;
                Chart.AxesMarginsDown = AxesMarginsDown;
                Chart.AxesMarginsLeft = AxesMarginsLeft;
                Chart.AxesMarginsRight = AxesMarginsRight;

                // Title settings
                if (Title != null)
                {
                    Chart.Title = Title;
                    Chart.TitleFontBold = TitleFontBold;
                    Chart.TitleFontSize = TitleFontSize;
                    Chart.TitleFontColor = TitleFontColor;
                }

                // This set the distance of the labels from the chart center (Pie Chart)
                Chart.LabelDistance = LabelDistance;

                // Font Settings
                Chart.FontName = FontName;
                Chart.LabelFontSize = LabelFontSize;
                Chart.LabelFontColor = LabelFontColor;
                Chart.LabelBold = LabelBold;

                // Set file directory save path 
                Chart.OutputFolderPath = OutputFolderPath;

                Chart.Format = Format;
                Pie myPie = new();
                WriteObject(myPie.Chart(Values, Labels, Filename, Width, Height));
            }
            else
            {
                WriteObject("Please provide both Values and Labels parameters.");
            }
        }
    }
}

class ValidatePath : ValidateArgumentsAttribute
{
    protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
    {
        string path = (string)arguments;
        if (Directory.Exists(path) == false)
        {
            throw new ArgumentException("Error: Directory Not Found Exception");
        }
    }
}