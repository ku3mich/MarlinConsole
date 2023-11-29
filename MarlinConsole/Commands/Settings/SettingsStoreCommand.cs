using MarlinConsole.Models;
using Spectre.Console;

namespace MarlinConsole.Commands.Settings;

[Mark(Register.Singleton)]
public class SettingsStoreCommand(IAnsiConsole console, GCode gcode) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct) =>
        console.StatusAsync("Executing...", ct,
            async (ctx, ct) =>
            {
                var result = await gcode.ExecuteAsync("M500", Timeouts.Long, ct);
                console.SendMarkup(gcode.Colorize(gcode.StripEcho(gcode.StripOK(result))));
            });

    public string Help { get; } = "Reports settings";
}
