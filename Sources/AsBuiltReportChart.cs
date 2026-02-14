using AsBuiltReportChart.Enums;

namespace AsBuiltReportChart
{
    public static class MainEntry
    {
        public static void Main(string[] args)
        {
            double[] values = [3, 2];
            string[] labels = ["Cassa", "Carro"];

            Chart.EnableLegend = true;
            Chart.LegendFontColor = BasicColors.Black;
            Chart.Title = "Aggregate Usage";
            Chart.TitleFontBold = false;
            Chart.TitleFontSize = 18;
            Chart.TitleFontColor = BasicColors.Black;
            Chart.LabelXAxis = "Aggregates";
            Chart.LabelYAxis = "Usage";
            Chart.LabelBold = false;
            Chart.LabelFontSize = 14;
            Chart.LabelFontColor = BasicColors.Black;
            Chart.ChartBorderStyle = BorderStyles.Dotted;
            Chart.ChartBorderColor = BasicColors.DarkGreen;
            Chart.ChartBorderSize = 2;
            Chart.EnableChartBorder = true;
            Chart.LegendOrientation = Orientations.Horizontal;
            Chart.LegendAlignment = Alignments.UpperCenter;
            Chart.LegendBorderSize = 0;
            Chart.AxesMarginsTop = 0.2;

            // Chart.ColorPalette = ColorPalettes.Dark;

            Chart.AreaOrientation = Orientations.Horizontal;
            Pie PieBar = new();
            Chart.Format = Formats.png;

            Chart.EnableCustomColorPalette = true;
            Chart.InvertCustomColorPalette = true;
            Chart.CustomColorPalette = ["#DFF0D0", "#FFF4C7", "#FEDDD7", "#878787", "#77a898", "#5e9584", "#458370", "#2a715d", "#005f4b"];

            PieBar.Chart(values, labels, width: 600, height: 400, filename: "PieChartExample");
        }
    }
}