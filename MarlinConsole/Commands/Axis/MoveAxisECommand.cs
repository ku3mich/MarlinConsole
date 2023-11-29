using MarlinConsole.Commands.Models;
using MarlinConsole.Models;

namespace MarlinConsole.Commands.Axis;

[Mark(Register.Singleton)]
public class MoveAxisECommand(SelectMoveStepCommand moveSelect, MoveAxisCommand moveAxis, GCode gcode) : IAsyncCommand, IHasHelp
{
    public string Help { get; } = "Interactively moves along E Axis";

    public async Task ExecuteAsync(CancellationToken ct)
    {
        var step = await moveSelect.ExecuteAsync(ct);
        if (step == null)
            return;

        await gcode.ExecuteAsync("M302 S0", Timeouts.Small, ct);
        await gcode.ExecuteAsync("G92 E0", Timeouts.Small, ct);

        var model = new MoveModel(Axises.E, -300, 300, step.Value);

        await moveAxis.ExecuteAsync(model, ct);
    }
}