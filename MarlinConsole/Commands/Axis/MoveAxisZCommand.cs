using MarlinConsole.Commands.Models;
using MarlinConsole.Models;

namespace MarlinConsole.Commands.Axis;

[Mark(Register.Singleton)]
public class MoveAxisZCommand(SelectMoveStepCommand moveSelect, MoveAxisCommand moveAxis) : IAsyncCommand, IHasHelp
{
    public string Help { get; } = "Interactively moves along Z Axis";

    public async Task ExecuteAsync(CancellationToken ct)
    {
        var step = await moveSelect.ExecuteAsync(ct);
        if (step == null)
            return;

        var model = new MoveModel(Axises.Z, -20, 300, step.Value);

        await moveAxis.ExecuteAsync(model, ct);
    }
}
