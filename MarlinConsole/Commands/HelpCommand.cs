using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace MarlinConsole.Commands;

[Mark(Register.Singleton)]
public class HelpCommand(IAnsiConsole console, CommandsProvider commands, IServiceProvider serviceProvider) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct)
    {
        foreach (var (cmd, cmdType) in commands.Commands)
        {
            var svc = serviceProvider.GetRequiredService(cmdType) as IHasHelp;
            var help = svc == null ? $"['{cmdType.Name}' has no help available]" : svc.Help;
            console.WriteLine($"{cmd}: {help}");
        }

        return Task.CompletedTask;
    }

    public string Help { get; } = "Displays Help";
}
