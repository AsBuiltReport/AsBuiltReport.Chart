using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using AsBuiltReportChart.Enums;

namespace AsBuiltReportChart
{
    internal class SignalChart : Chart
    {
        public object Chart(List<double[]> values, string[] labels, double xOffset = 0, double period = 1.0, bool dateTimeTicksBottom = false, string filename = "output", int width = 400, int height = 300)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("Error: Values cannot be null or empty.");
            }

            Plot myPlot = new Plot();

            if (EnableCustomColorPalette)
            {
                if (_customColorPalette != null && _customColorPalette.Length > 0)
                {
                    myPlot.Add.Palette = colorPalette = new ScottPlot.Palettes.Custom(_customColorPalette);
                }
                else
                {
                    throw new Exception("CustomColorPalette is empty. Please provide valid color values.");
                }
            }
            else
            {
                if (colorPalette != null)
                {
                    myPlot.Add.Palette = colorPalette;
                }
            }

            // Add each signal line
            for (int i = 0; i < values.Count; i++)
            {
                var sig = myPlot.Add.Signal(values[i]);
                sig.Data.XOffset = xOffset;
                sig.Data.Period = period;

                if (colorPalette != null)
                {
                    sig.Color = colorPalette.GetColor(i);
                }

                if (labels != null && i < labels.Length && EnableLegend)
                {
                    sig.LegendText = labels[i];
                }
            }

            // Enable DateTime ticks on the X axis if requested
            if (dateTimeTicksBottom)
            {
                myPlot.Axes.DateTimeTicksBottom();
            }

            // Set X and Y axis label settings
            myPlot.Axes.Bottom.Label.Text = LabelXAxis;
            myPlot.Axes.Bottom.Label.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.Label.FontName = FontName;
            myPlot.Axes.Bottom.Label.Bold = LabelBold;

            myPlot.Axes.Bottom.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.TickLabelStyle.FontName = FontName;

            myPlot.Axes.Left.Label.Text = LabelYAxis;
            myPlot.Axes.Left.Label.FontSize = LabelFontSize;
            myPlot.Axes.Left.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Left.Label.FontName = FontName;
            myPlot.Axes.Left.Label.Bold = LabelBold;

            myPlot.Axes.Left.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Left.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Left.TickLabelStyle.FontName = FontName;

            // Hide unnecessary plot components
            myPlot.HideGrid();
            myPlot.Axes.Top.IsVisible = false;
            myPlot.Axes.Right.IsVisible = false;

            if (EnableLegend)
            {
                myPlot.ShowLegend();

                // Legend Font Properties
                myPlot.Legend.FontName = FontName;
                myPlot.Legend.FontSize = LegendFontSize;
                myPlot.Legend.FontColor = GetDrawingColor(LegendFontColor);

                // Legend box Style Properties
                myPlot.Legend.OutlineColor = GetDrawingColor(LegendBorderColor);
                myPlot.Legend.OutlineWidth = LegendBorderSize;
                myPlot.Legend.OutlinePattern = LegendBorderStyleMap[LegendBorderStyle];
                myPlot.Legend.Orientation = LegendOrientationMap[LegendOrientation];
                myPlot.Legend.Alignment = LegendAlignmentMap[LegendAlignment];
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

            // Set title properties
            if (Title != null)
            {
                myPlot.Title(Title);
                myPlot.Axes.Title.Label.FontSize = TitleFontSize;
                myPlot.Axes.Title.Label.ForeColor = GetDrawingColor(TitleFontColor);
                myPlot.Axes.Title.Label.Bold = TitleFontBold;
                myPlot.Axes.Title.Label.FontName = FontName;
            }

            // Set margins settings
            myPlot.Axes.Margins(left: AxesMarginsLeft, right: AxesMarginsRight, bottom: AxesMarginsDown, top: AxesMarginsTop);

            // Set filepath
            string Filepath = _outputFolderPath ?? Directory.GetCurrentDirectory();

            // Save Plot
            return SaveInFormat(myPlot, width, height, Filepath, filename, Format);
        }
    }
}
