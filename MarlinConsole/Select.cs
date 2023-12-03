using Spectre.Console;
using Spectre.Selection;

namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public class Select(IAnsiConsole console)
{
    public string? Menu()
    {
        var prompt = new SimpleSelectPrompt()
            .Options(
                (Items.Move.Menu, "Move"),
                (Items.Settings.Menu, "Settings")
            );

        return console.Prompt(prompt);
    }

    public string? Move()
    {
        var prompt = new SimpleSelectPrompt()
            .Options(
                (Items.Move.Home, "Home"),
                (Items.Move.AxisMenu, "Move Axis"),
                (Items.Move.AdjustProbe, "Adjust Probe"));

        return console.Prompt(prompt);
    }

    public string? AxisMove()
    {
        var prompt = new SimpleSelectPrompt()
            .Options(
                (Items.Move.Axis.E, "E"),
                (Items.Move.Axis.Z, "Z"));

        return console.Prompt(prompt);
    }

    public string? Settings()
    {
        var prompt = new SimpleSelectPrompt()
            .Options(
                (Items.Settings.Report, "Report settings"),
                (Items.Settings.Speeds, "Report speeds"),
                (Items.Settings.Save, "Save settings"));

        return console.Prompt(prompt);
    }
}
