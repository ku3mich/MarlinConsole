using MarlinConsole.Models;
using Spectre.Console;

namespace MarlinConsole.Commands.Settings;

[Mark(By.Register, Injects.Singleton)]
public class SettingsReportCommand(IAnsiConsole console, GCode gcode) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct) =>
        console.StatusAsync("Reading...", ct,
            async (ctx, ct) =>
            {
                var result = await gcode.ExecuteAsync("M503", Timeouts.Long, ct);
                console.SendMarkup(gcode.Colorize(gcode.StripEcho(gcode.StripOK(result))));
            });

    public string Help { get; } = "Reports settings";
}

