using ScottPlot;
using AsBuiltReportChart.Enums;
namespace AsBuiltReportChart;

public class StackedBar : Chart
{
    static StackedBar() { }
    public object Chart(List<double[]> values, string[] labels, string[] categoryNames, string filename = "output", int width = 400, int height = 300)
    {
        if (values.Count == labels.Length)
        {
            Plot myPlot = new();

            if (EnableCustomColorPalette)
            {
                if (_customColorPalette is not null and not [] && _customColorPalette.Length > 0)
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
                // Set ScottPlot native color palette
                if (colorPalette is not null)
                {
                    myPlot.Add.Palette = colorPalette;
                }
            }

            // Set X and Y axis label settings
            myPlot.Axes.Bottom.Label.Text = AreaOrientation switch
            {
                Orientations.Horizontal => LabelYAxis,
                Orientations.Vertical => LabelXAxis,
                _ => ""
            };
            myPlot.Axes.Bottom.Label.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.Label.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.Label.FontName = FontName;

            myPlot.Axes.Bottom.TickLabelStyle.FontSize = LabelFontSize;
            myPlot.Axes.Bottom.TickLabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
            myPlot.Axes.Bottom.TickLabelStyle.FontName = FontName;
            myPlot.Axes.Bottom.Label.Bold = LabelBold;

            // myPlot.Axes.Bottom.TickLabelStyle.Rotation = -10;

            myPlot.Axes.Left.Label.Text = AreaOrientation switch
            {
                Orientations.Horizontal => LabelXAxis,
                Orientations.Vertical => LabelYAxis,
                _ => ""
            };
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
            for (int x = 0; x < values.Count; x++)
            {
                double nextBarBase = 0;
                for (int i = 0; i < values[x].Length; i++)
                {
                    if (colorPalette is not null)
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
            ScottPlot.TickGenerators.NumericManual tickGen = new();

            // assign labels to each bar
            if (AreaOrientation == Orientations.Vertical)
            {
                for (var i = 0; i < categoryNames.Length; i++)
                {
                    if (colorPalette is not null && EnableLegend)
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
                    if (colorPalette is not null && EnableLegend)
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

            bar.Horizontal = AreaOrientation switch
            {
                Orientations.Horizontal => true,
                _ => false
            };


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
                myPlot.FigureBorder = new()
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
            if (AreaOrientation == Orientations.Horizontal)
            {
                myPlot.Axes.Margins(left: AxesMarginsLeft, right: AxesMarginsRight, bottom: AxesMarginsDown, top: AxesMarginsTop);
            }
            else
            {
                myPlot.Axes.Margins(left: AxesMarginsLeft, right: AxesMarginsRight, bottom: AxesMarginsDown, top: AxesMarginsTop);
            }

            // Set filepath

            string Filepath = _outputFolderPath ?? Directory.GetCurrentDirectory();

            // Set filename
            return SaveInFormat(myPlot, width, height, Filepath, filename, Format);
        }
        else
        {
            throw new ArgumentException("Error: Values and labels must be equal.");
        }
    }
}


