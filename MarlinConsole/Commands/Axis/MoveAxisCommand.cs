using MarlinConsole.Commands.Models;
using MarlinConsole.Models;
using Spectre.Console;
using Spectre.Spinner;

namespace MarlinConsole.Commands.Axis;

[Mark(By.Register, Injects.Singleton)]
public class MoveAxisCommand(IAnsiConsole console, GCode gcode, GCodeParser gcodeParser) : IAsyncAction<MoveModel>
{
    public async Task ExecuteAsync(MoveModel model, CancellationToken ct)
    {
        await gcode.ExecuteAsync("G90", Timeouts.Small, ct);
        var coords = await gcode.ExecuteAsync("M114", Timeouts.Small, ct);

        if (coords == null)
            // TODO: warn
            return;

        var lastMove = gcodeParser.ExtractDouble(coords, model.Axis);

        if (!lastMove.HasValue)
            return;

        var spinner = new SpinnerPrompt()
            .Min(model.Min)
            .Max(model.Max)
            .Value(lastMove.Value)
            .Step(model.Step)
            .OnChangeAsync(async s =>
            {
                var result = await gcode.ExecuteAsync($"G0 {model.Axis}{Math.Round(s, 3, MidpointRounding.AwayFromZero)}", Timeouts.Small, ct);

                if (result == null)
                    return s;

                var stripped = gcode.StripOK(result);

                console.Send(stripped);

                return s;
            })
            .Prompt($"{model.Axis}: ");

        var value = console.Prompt(spinner);
    }
}
