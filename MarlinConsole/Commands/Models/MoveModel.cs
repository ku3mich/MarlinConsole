namespace MarlinConsole.Commands.Models;

public class MoveModel(string axis, double min, double max, double step)
{
    public string Axis { get; set; } = axis;
    public double Step { get; set; } = step;
    public double Min { get; set; } = min;
    public double Max { get; set; } = max;
}
