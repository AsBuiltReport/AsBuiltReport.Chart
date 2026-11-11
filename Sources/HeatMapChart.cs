using System;
using System.IO;
using System.Linq;
using ScottPlot;

namespace AsBuiltReportChart
{
    internal class HeatMap : Chart
    {
        public object Chart(double[][] values, string[] rowLabels, string[] columnLabels, string filename = "output", int width = 400, int height = 300)
        {
            if (values == null || values.Length == 0 || values[0] == null || values[0].Length == 0)
            {
                throw new ArgumentException("Error: Values cannot be null or empty.");
            }

            int rows = values.Length;
            int columns = values[0].Length;

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                if (values[rowIndex] == null || values[rowIndex].Length != columns)
                {
                    throw new ArgumentException("Error: All rows in Values must have the same length.");
                }
            }

            if (rowLabels != null && rowLabels.Length != rows)
            {
                throw new ArgumentException("Error: Row labels length must match the number of rows.");
            }

            if (columnLabels != null && columnLabels.Length != columns)
            {
                throw new ArgumentException("Error: Column labels length must match the number of columns.");
            }

            Plot myPlot = new Plot();
            double[,] matrix = new double[rows, columns];

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    matrix[rowIndex, columnIndex] = values[rowIndex][columnIndex];
                }
            }

            var heatmap = myPlot.Add.Heatmap(matrix);

            if (EnableLegend)
            {
                var colorBar = myPlot.Add.ColorBar(heatmap, ScottPlot.Edge.Right);
                colorBar.Width = 20;
                colorBar.Label = "Value";
            }

            myPlot.Axes.Left.Label.Text = LabelYAxis;
            myPlot.Axes.Bottom.Label.Text = LabelXAxis;
            myPlot.Axes.Left.Label.FontSize = LabelFontSize;
            myPlot.Axes.Left.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Left.Label.FontName = FontName;
            myPlot.Axes.Left.Label.Bold = LabelBold;
            myPlot.Axes.Bottom.Label.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.Label.FontName = FontName;
            myPlot.Axes.Bottom.Label.Bold = LabelBold;

            myPlot.Axes.Left.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Left.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Left.TickLabelStyle.FontName = FontName;
            myPlot.Axes.Bottom.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.TickLabelStyle.FontName = FontName;

            if (rowLabels != null && rowLabels.Length > 0)
            {
                double[] positions = Enumerable.Range(0, rowLabels.Length).Select(i => (double)i).ToArray();
                myPlot.Axes.Left.SetTicks(positions, rowLabels);
            }

            if (columnLabels != null && columnLabels.Length > 0)
            {
                double[] positions = Enumerable.Range(0, columnLabels.Length).Select(i => (double)i).ToArray();
                myPlot.Axes.Bottom.SetTicks(positions, columnLabels);
            }

            myPlot.HideGrid();
            myPlot.Axes.Top.IsVisible = false;
            myPlot.Axes.Right.IsVisible = false;

            if (Title != null)
            {
                myPlot.Title(Title);
                myPlot.Axes.Title.Label.FontSize = TitleFontSize;
                myPlot.Axes.Title.Label.ForeColor = GetDrawingColor(TitleFontColor);
                myPlot.Axes.Title.Label.Bold = TitleFontBold;
                myPlot.Axes.Title.Label.FontName = FontName;
            }

            myPlot.Axes.Margins(left: AxesMarginsLeft, right: AxesMarginsRight, bottom: AxesMarginsDown, top: AxesMarginsTop);

            if (FigureBackgroundColor.HasValue)
            {
                myPlot.FigureBackground.Color = GetDrawingColor(FigureBackgroundColor.Value);
            }

            if (DataBackgroundColor.HasValue)
            {
                myPlot.DataBackground.Color = GetDrawingColor(DataBackgroundColor.Value);
            }

            if (EnableChartBorder)
            {
                myPlot.FigureBorder = new LineStyle()
                {
                    Color = GetDrawingColor(ChartBorderColor),
                    Width = ChartBorderSize,
                    Pattern = ChartBorderStyleMap[ChartBorderStyle],
                };
            }

            ApplyWatermark(myPlot);

            string filepath = _outputFolderPath ?? Directory.GetCurrentDirectory();
            return SaveInFormat(myPlot, width, height, filepath, filename, Format);
        }
    }
}
