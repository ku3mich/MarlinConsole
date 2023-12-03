using System.Text.RegularExpressions;

namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public class GCodeParser
{
    public int? ExtractInt(string code, string parameter)
    {
        var v = ExtractValue(code, parameter);
        if (v == null)
            return null;

        return int.Parse(v);
    }

    public double? ExtractDouble(string code, string parameter)
    {
        var v = ExtractValue(code, parameter);
        if (v == null)
            return null;

        return double.Parse(v);
    }

    // X:0.0000 Y:0.0000 Z:275.5441 E:0.0000 Count A:81684B:81684C:81684
    public string? ExtractValue(string code, string parameter)
    {
        var rex = new Regex($"{parameter}:([0-9.]+)");
        var matches = rex.Matches(code);

        if (matches.Count == 1 && matches[0].Groups.Count == 2)
            return matches[0].Groups[1].Value;

        return null;
    }
}
