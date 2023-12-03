using Spectre.Console;

namespace MarlinConsole.Commands;

[Mark(By.Register, Injects.Singleton)]
public class StateCommand(IAnsiConsole console, State state) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct)
    {
        console.WriteLine($"state: device: [{state.Device}] baud: [{state.BaudRate}] connected: {state.IsConnected}");

        return Task.CompletedTask;
    }

    public string Help { get; } = "State";
}
