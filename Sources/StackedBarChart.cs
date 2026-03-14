using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using AsBuiltReportChart.Enums;
namespace AsBuiltReportChart
{
    internal class StackedBar : Chart
    {
        static StackedBar() { }
        public object Chart(List<double[]> values, string[] labels, string[] categoryNames, string filename = "output", int width = 400, int height = 300)
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
                // Set ScottPlot native color palette
                if (colorPalette != null)
                {
                    myPlot.Add.Palette = colorPalette;
                }
            }

            // Set X and Y axis label settings
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

            myPlot.Axes.Bottom.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.TickLabelStyle.FontName = FontName;
            myPlot.Axes.Bottom.Label.Bold = LabelBold;

            // myPlot.Axes.Bottom.TickLabelStyle.Rotation = -10;

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

            // create bars
            var bars = new List<ScottPlot.Bar>();
            // assign values and colors to each bar
            // Validate that values.Length matches labels.Length
            // This validation is necessary to ensure that each set of values corresponds to a label, which is crucial for accurate representation in the stacked bar chart. If the lengths do not match, it indicates a mismatch in the data structure, leading to potential errors in plotting and misinterpretation of the chart.
            if (values.Count != labels.Length)
            {
                throw new ArgumentException("Error: Value sets and Label length must be equal.");
            }
            // Validate that each set of values has the same length as category names
            // This validation ensures that each category in the stacked bar chart has a corresponding value for each label. If the lengths do not match, it indicates an inconsistency in the data structure, which can lead to errors in plotting and misrepresentation of the chart. Each set of values must align with the category names to accurately reflect the data in the stacked bar chart.
            foreach (var valueSet in values)
            {
                if (valueSet.Length != categoryNames.Length)
                {
                    throw new ArgumentException("Error: Each set of values must have the same length as category names.");
                }
            }

            for (int x = 0; x < values.Count; x++)
            {
                double nextBarBase = 0;
                for (int i = 0; i < values[x].Length; i++)
                {
                    if (colorPalette != null)
                    {
                        bars.Add(new ScottPlot.Bar
                        {
                            Position = x,
                            Value = nextBarBase + values[x][i],
                            ValueBase = nextBarBase,
                            FillColor = colorPalette.GetColor(i),
                            Label = $"{values[x][i]}",
                            CenterLabel = true,
                        });
                        nextBarBase += values[x][i];
                    }
                }
            }

            // add bars to plot
            var bar = myPlot.Add.Bars(bars);

            // Customize bars label style, including color
            bar.ValueLabelStyle.FontName = FontName;
            bar.ValueLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            bar.ValueLabelStyle.Bold = LabelBold;
            bar.ValueLabelStyle.FontSize = LabelFontSize;

            // set each slice value to its label
            ScottPlot.TickGenerators.NumericManual tickGen = new ScottPlot.TickGenerators.NumericManual();

            // assign labels to each bar
            if (AreaOrientation == Orientations.Vertical)
            {
                for (var i = 0; i < categoryNames.Length; i++)
                {
                    if (colorPalette != null && EnableLegend)
                    {
                        myPlot.Legend.ManualItems.Add(new LegendItem()
                        {
                            LabelText = categoryNames[i],
                            FillColor = colorPalette.GetColor(i)
                        });
                    }
                }

                for (var i = 0; i < labels.Length; i++)
                {
                    // set ticks
                    tickGen.AddMajor(i, labels[i]);
                }
                myPlot.Axes.Bottom.TickGenerator = tickGen;
            }
            else
            {
                for (var i = 0; i < categoryNames.Length; i++)
                {
                    if (colorPalette != null && EnableLegend)
                    {
                        myPlot.Legend.ManualItems.Add(new LegendItem()
                        {
                            LabelText = categoryNames[i],
                            FillColor = colorPalette.GetColor(i)
                        });
                    }
                }

                for (var i = 0; i < labels.Length; i++)
                {
                    // set ticks
                    tickGen.AddMajor(i, labels[i]);
                }
                // set ticks for horizontal orientation
                myPlot.Axes.Left.TickGenerator = tickGen;
            }

            bar.Horizontal = (AreaOrientation == Orientations.Horizontal);


            // hide unnecessary plot components
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
                    // Set chart border properties
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

            // Set background colors
            if (FigureBackgroundColor.HasValue)
            {
                myPlot.FigureBackground.Color = GetDrawingColor(FigureBackgroundColor.Value);
            }
            if (DataBackgroundColor.HasValue)
            {
                myPlot.DataBackground.Color = GetDrawingColor(DataBackgroundColor.Value);
            }

            // Set axis limits if there is only one value to prevent auto-scaling issues
            if (values.Count == 1)
            {
                // Compute the stacked total for the single bar
                double stackedTotal = 0;
                if (values[0] != null)
                {
                    foreach (double v in values[0])
                    {
                        stackedTotal += v;
                    }
                }

                // Ensure a non-zero span even if stackedTotal is zero or negative
                double paddingFraction = 0.1;
                double effectiveTotal = stackedTotal > 0 ? stackedTotal * (1 + paddingFraction) : 1.0;

                double xMin, xMax, yMin, yMax;
                double barIndex = 0; // single bar at index 0

                if (AreaOrientation == Orientations.Horizontal)
                {
                    // Horizontal bars: X is value, Y is category (bar index)
                    xMin = 0;
                    xMax = effectiveTotal;
                    yMin = barIndex - 0.5;
                    yMax = barIndex + 0.5;
                }
                else
                {
                    // Vertical bars: X is category (bar index), Y is value
                    xMin = barIndex - 0.5;
                    xMax = barIndex + 0.5;
                    yMin = 0;
                    yMax = effectiveTotal;
                }

                myPlot.Axes.SetLimits(xMin, xMax, yMin, yMax);
            }

            // Apply watermark if enabled
            ApplyWatermark(myPlot);

            // Set filepath
            string Filepath = _outputFolderPath ?? Directory.GetCurrentDirectory();

            // Set filename
            return SaveInFormat(myPlot, width, height, Filepath, filename, Format);
        }
    }
}


