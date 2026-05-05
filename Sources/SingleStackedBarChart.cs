using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using AsBuiltReportChart.Enums;
namespace AsBuiltReportChart
{
    internal class SingleStackedBar : Chart
    {
        static SingleStackedBar() { }
        public object Chart(double[] values, string label, string[] categoryNames, string filename = "output", int width = 400, int height = 300)
        {
            Plot myPlot = new Plot();

            if (EnableCustomColorPalette)
            {
                if (_customColorPalette != null && _customColorPalette.Length > 0)
                {
                    myPlot.Add.Palette = colorPalette = new ScottPlot.Palettes.Custom(_customColorPalette);
                }
                else
                {
                    throw new InvalidOperationException("CustomColorPalette is empty. Please provide valid color values.");
                }
            }
            else
            {
                if (colorPalette != null)
                {
                    myPlot.Add.Palette = colorPalette;
                }
            }

            // Validate that values.Length matches categoryNames.Length
            if (values.Length != categoryNames.Length)
            {
                throw new ArgumentException("Error: Values and category names length must be equal.");
            }

            // Build the single stacked bar
            var bars = new List<ScottPlot.Bar>();
            double nextBarBase = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (colorPalette != null)
                {
                    bars.Add(new ScottPlot.Bar
                    {
                        Position = 0,
                        Value = nextBarBase + values[i],
                        ValueBase = nextBarBase,
                        FillColor = colorPalette.GetColor(i),
                        Label = $"{values[i]:0.##}{ValueSuffix}",
                        CenterLabel = true,
                    });
                    nextBarBase += values[i];
                }
            }

            var bar = myPlot.Add.Bars(bars);

            // Customize bar segment label style
            bar.ValueLabelStyle.FontName = FontName;
            bar.ValueLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            bar.ValueLabelStyle.Bold = LabelBold;
            bar.ValueLabelStyle.FontSize = LabelFontSize;

            // Set X-axis label settings — swap axis text based on orientation
            if (AreaOrientation == Orientations.Horizontal)
            {
                myPlot.Axes.Bottom.Label.Text = LabelYAxis;
            }
            else if (AreaOrientation == Orientations.Vertical)
            {
                myPlot.Axes.Bottom.Label.Text = LabelXAxis;
            }
            else
            {
                myPlot.Axes.Bottom.Label.Text = "";
            }
            myPlot.Axes.Bottom.Label.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.Label.FontName = FontName;
            myPlot.Axes.Bottom.Label.Bold = LabelBold;

            myPlot.Axes.Bottom.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.TickLabelStyle.FontName = FontName;

            // Set Y-axis label settings — swap axis text based on orientation
            if (AreaOrientation == Orientations.Horizontal)
            {
                myPlot.Axes.Left.Label.Text = LabelXAxis;
            }
            else if (AreaOrientation == Orientations.Vertical)
            {
                myPlot.Axes.Left.Label.Text = LabelYAxis;
            }
            else
            {
                myPlot.Axes.Left.Label.Text = "";
            }
            myPlot.Axes.Left.Label.FontSize = LabelFontSize;
            myPlot.Axes.Left.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Left.Label.FontName = FontName;
            myPlot.Axes.Left.Label.Bold = LabelBold;

            myPlot.Axes.Left.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Left.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Left.TickLabelStyle.FontName = FontName;

            // Category tick (single bar label) and value tick generators depend on orientation
            var tickGenCategory = new ScottPlot.TickGenerators.NumericManual();
            tickGenCategory.AddMajor(0, label);

            var tickGenValue = new ScottPlot.TickGenerators.NumericAutomatic();
            tickGenValue.LabelFormatter = value => $"{value:0.##}{ValueSuffix}";

            if (AreaOrientation == Orientations.Vertical)
            {
                // Vertical: categories on X (bottom), values on Y (left)
                myPlot.Axes.Bottom.TickGenerator = tickGenCategory;
                myPlot.Axes.Left.TickGenerator = tickGenValue;
            }
            else
            {
                // Horizontal: categories on Y (left), values on X (bottom)
                myPlot.Axes.Left.TickGenerator = tickGenCategory;
                myPlot.Axes.Bottom.TickGenerator = tickGenValue;
            }

            // Legend
            if (EnableLegend)
            {
                for (int i = 0; i < categoryNames.Length; i++)
                {
                    if (colorPalette != null)
                    {
                        myPlot.Legend.ManualItems.Add(new LegendItem()
                        {
                            LabelText = categoryNames[i],
                            FillColor = colorPalette.GetColor(i)
                        });
                    }
                }

                myPlot.ShowLegend();

                myPlot.Legend.FontName = FontName;
                myPlot.Legend.FontSize = LegendFontSize;
                myPlot.Legend.FontColor = GetDrawingColor(LegendFontColor);

                myPlot.Legend.OutlineColor = GetDrawingColor(LegendBorderColor);
                myPlot.Legend.OutlineWidth = LegendBorderSize;
                myPlot.Legend.OutlinePattern = LegendBorderStyleMap[LegendBorderStyle];

                myPlot.Legend.Orientation = LegendOrientationMap[LegendOrientation];
                myPlot.Legend.Alignment = LegendAlignmentMap[LegendAlignment];
            }

            // Hide unnecessary plot components
            myPlot.HideGrid();
            myPlot.Axes.Top.IsVisible = false;
            myPlot.Axes.Right.IsVisible = false;

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

            // Set margins
            // myPlot.Axes.Margins(left: AxesMarginsLeft, right: AxesMarginsRight, bottom: AxesMarginsDown, top: AxesMarginsTop);

            // Set background colors
            if (FigureBackgroundColor.HasValue)
            {
                myPlot.FigureBackground.Color = GetDrawingColor(FigureBackgroundColor.Value);
            }
            if (DataBackgroundColor.HasValue)
            {
                myPlot.DataBackground.Color = GetDrawingColor(DataBackgroundColor.Value);
            }

            // Set bar orientation
            bar.Horizontal = (AreaOrientation == Orientations.Horizontal);

            // Compute cumulative range and set axis limits
            double cumulative = 0;
            double minSum = 0;
            double maxSum = 0;

            foreach (double v in values)
            {
                cumulative += v;
                if (cumulative < minSum) minSum = cumulative;
                if (cumulative > maxSum) maxSum = cumulative;
            }

            double minVal = Math.Min(0, minSum);
            double maxVal = Math.Max(0, maxSum);

            if (minVal == maxVal)
            {
                double pad = (minVal == 0) ? 1.0 : Math.Abs(minVal) * 0.1;
                minVal -= pad;
                maxVal += pad;
            }
            else
            {
                double range = maxVal - minVal;
                double pad = range * 0.1;
                minVal -= pad;
                maxVal += pad;
            }

            // Single bar at position 0 — swap value/category axes for orientation
            double barIndex = 0;
            if (AreaOrientation == Orientations.Horizontal)
            {
                // Horizontal: value axis is X, category axis is Y
                myPlot.Axes.SetLimits(minVal, maxVal, barIndex - 2, barIndex + 2);
            }
            else
            {
                // Vertical: category axis is X, value axis is Y
                myPlot.Axes.SetLimits(barIndex - 2, barIndex + 2, minVal, maxVal);
            }

            // Apply watermark if enabled
            ApplyWatermark(myPlot);

            // Set filepath
            string Filepath = _outputFolderPath ?? Directory.GetCurrentDirectory();

            return SaveInFormat(myPlot, width, height, Filepath, filename, Format);
        }
    }
}
