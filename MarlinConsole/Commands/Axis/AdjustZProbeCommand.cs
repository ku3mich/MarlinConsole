using Spectre.Console;

namespace MarlinConsole.Commands.Axis;

[Mark(By.Register, Injects.Singleton)]
public class AdjustZProbeCommand(IAnsiConsole console) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct)
    {
        console.WriteLine("AdjustZProbeCommand");

        return Task.CompletedTask;
    }

    public string Help { get; } = "AdjustZProbeCommand";
}
