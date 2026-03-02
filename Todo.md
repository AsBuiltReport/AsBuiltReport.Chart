- [ ] Add option to set background color of the chart area
  - [ ] https://scottplot.net/cookbook/5/Styling/BackgroundColors/

```ScottPlot.Plot myPlot = new();
// setup a plot with sample data
myPlot.Add.Signal(Generate.Sin(51));
myPlot.Add.Signal(Generate.Cos(51));
myPlot.XLabel("Horizontal Axis");
myPlot.YLabel("Vertical Axis");

// some items must be styled directly
myPlot.FigureBackground.Color = Colors.Navy;
myPlot.DataBackground.Color = Colors.Navy.Darken(0.1);
myPlot.Grid.MajorLineColor = Colors.Navy.Lighten(0.1);

// some items have helper methods to configure multiple properties at once
myPlot.Axes.Color(Colors.Navy.Lighten(0.8));

myPlot.SavePng("demo.png", 400, 300);
```
