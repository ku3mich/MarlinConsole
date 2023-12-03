using Spectre.Console;

namespace MarlinConsole.Commands;

[Mark(By.Register, Injects.Singleton)]
public class ClearCommand(IAnsiConsole console) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct)
    {
        console.Clear();

        return Task.CompletedTask;
    }

    public string Help { get; } = "Clears screen";
}
