using ScottPlot;
using System;
using System.IO;
using System.Collections.Generic;
namespace AsBuiltReportChart
{
    internal class Radar : Chart
    {
        public Radar() { }
        public object Chart(List<double[]> values, string[] labels, string filename = "output", int width = 400, int height = 300)
        {
            if (values.Count == labels.Length)
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

                var radar = myPlot.Add.Radar(values);

                // set each slice value to its label
                if (EnableLegend)
                {
                    for (var i = 0; i < radar.Series.Count; i++)
                    {
                        radar.Series[i].LegendText = labels[i];
                    }
                }

                // hide unnecessary plot components
                myPlot.Axes.Frameless();
                myPlot.HideGrid();

                if (EnableLegend)
                {
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

                // Set background colors
                if (FigureBackgroundColor.HasValue)
                {
                    myPlot.FigureBackground.Color = GetDrawingColor(FigureBackgroundColor.Value);
                }
                if (DataBackgroundColor.HasValue)
                {
                    myPlot.DataBackground.Color = GetDrawingColor(DataBackgroundColor.Value);
                }

                // Set the distance of the chart area elements (Radar Chart)
                // radar.SliceLabelDistance = _labelDistance;

                // Apply watermark if enabled
                ApplyWatermark(myPlot);

                // Set filetpath to save
                string Filepath = _outputFolderPath ?? Directory.GetCurrentDirectory();

                // Set filename
                return SaveInFormat(myPlot, width, height, Filepath, filename, Format);
            }
            else
            {
                throw new ArgumentException("Error: Values and labels must be equal.");
            }
        }
        public object Chart(List<double[]> values, string[] labels, string[] categoryNames, string filename = "output", int width = 400, int height = 300)
        {
            if (values.Count == labels.Length)
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

                var radar = myPlot.Add.Radar(values);

                // set each slice value to its label
                if (EnableLegend)
                {
                    for (var i = 0; i < radar.Series.Count; i++)
                    {
                        radar.Series[i].LegendText = labels[i];
                    }
                }

                if (categoryNames != null)
                {
                    if (categoryNames.Length > 0 && categoryNames.Length == values[0].Length)
                    {
                        radar.PolarAxis.SetSpokes(categoryNames, length: SpokesLength);
                        // set each PolarAxis value to its label
                        for (var i = 0; i < radar.PolarAxis.Spokes.Count; i++)
                        {
                            radar.PolarAxis.Spokes[i].LabelStyle.FontSize = LabelFontSize;
                            radar.PolarAxis.Spokes[i].LabelStyle.ForeColor = GetDrawingColor(LabelFontColor);
                            radar.PolarAxis.Spokes[i].LabelStyle.Bold = LabelBold;
                            radar.PolarAxis.Spokes[i].LabelStyle.FontName = FontName;
                            radar.PolarAxis.Spokes[i].LabelPaddingFraction = LabelDistance;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Error: SpokeLabels must be provided and its length must match the number of values in each series.");
                    }
                }

                // hide unnecessary plot components
                myPlot.Axes.Frameless();
                myPlot.HideGrid();

                if (EnableLegend)
                {
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

                // Set background colors
                if (FigureBackgroundColor.HasValue)
                {
                    myPlot.FigureBackground.Color = GetDrawingColor(FigureBackgroundColor.Value);
                }
                if (DataBackgroundColor.HasValue)
                {
                    myPlot.DataBackground.Color = GetDrawingColor(DataBackgroundColor.Value);
                }

                // Set the distance of the chart area elements (Radar Chart)
                // radar.SliceLabelDistance = _labelDistance;

                // Apply watermark if enabled
                ApplyWatermark(myPlot);

                // Set filetpath to save
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
}


