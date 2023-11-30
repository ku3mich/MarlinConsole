using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Spinner;

public class SpinnerView : IRenderable
{
    public string Prompt { get; set; } = "Select:";
    public Style PromptStyle { get; set; } = Style.Plain;
    public Style ValueStyle { get; set; } = Style.Plain;

    public double Value { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SpinnerView(int decimals, double step, double min, double max)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        Decimals = decimals;
        Step = step;
        Min = min;
        Max = max;
    }

    string DisplayFormat { get; set; }

    private int _decimals;

    public int Decimals
    {
        get => _decimals;
        set
        {
            _decimals = value;
            DisplayFormat = $"0.{new string('0', _decimals)}";
        }
    }

    public double Min { get; set; }
    public double Max { get; set; }
    public double Step { get; set; }

    public void Decrease()
    {
        if (Value - Step >= Min)
            Value = Math.Round(Value - Step, 4, MidpointRounding.ToZero);
    }

    public void Increase()
    {
        if (Value + Step <= Max)
            Value = Math.Round(Value + Step, 4, MidpointRounding.ToZero);
    }

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var n = Render(options, maxWidth).Sum(s => s.CellCount());
        return new Measurement(n, n);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        yield return new Segment($"{Prompt} ", PromptStyle);
        yield return new Segment(string.Format($"{{0:{DisplayFormat}}}", Value));
    }
}
