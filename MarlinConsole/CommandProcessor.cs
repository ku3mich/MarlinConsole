using Spectre.Console;

namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public class CommandProcessor(IAnsiConsole console, CommandsProvider commandsProvider, AsyncCommandInvoker commandInvoker, GCode gcode)
{
    public async Task Process(string command, CancellationToken ct)
    {
        if (commandsProvider.Commands.TryGetValue(command, out Type? commandType))
        {
            await commandInvoker.InvokeAsync(commandType, ct);
            return;
        }

        if (!gcode.IsLikeGCode(command))
            return;

        var result = await gcode.ExecuteAsync(command, 3000, ct);

        if (!string.IsNullOrEmpty(result))
            console.Send(gcode.StripEcho(gcode.StripOK(result)));
    }
}
