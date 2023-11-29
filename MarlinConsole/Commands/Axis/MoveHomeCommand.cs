using Spectre.Console;

namespace MarlinConsole.Commands.Axis;

[Mark(Register.Singleton)]
public class MoveHomeCommand(IAnsiConsole console, GCode gcode) : IAsyncCommand, IHasHelp
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        await console.StatusAsync("Homing...", ct,
            async (ctx, ct) =>
            {
                var result = await gcode.ExecuteAsync("G28", 15000, ct);
                console.Send(gcode.StripOK(result));
            });
    }

    public string Help { get; } = "Move Home";
}
