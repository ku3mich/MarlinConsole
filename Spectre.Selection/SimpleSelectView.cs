using Spectre.Console.Rendering;

namespace Spectre.Selection;

public class SimpleSelectView : IRenderable
{
    public int ActiveIndex;
    public Dictionary<string, string> Options = [];

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var max = Options.Values.Max(s => s.Length);
        return new Measurement(max, max);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        int index = 0;
        foreach (var value in Options.Values)
        {
            if (index == ActiveIndex)
                yield return new Segment("> ");
            else
                yield return new Segment("  ");

            yield return new Segment(value);
            yield return Segment.LineBreak;

            index++;
        }
    }
}
