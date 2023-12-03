using Spectre.Console;
using Spectre.Selection;

namespace MarlinConsole.Commands.Axis;

[Mark(By.Register, Injects.Singleton)]
public class SelectMoveStepCommand(IAnsiConsole console) : IAsyncFunction<double?>
{
    public async Task<double?> ExecuteAsync(CancellationToken ct)
    {
        var select = new SimpleSelectPrompt()
            .Options(
                ("0.05", "0.05mm"),
                ("0.1", "0.1mm"),
                ("1", "1mm"),
                ("5", "5mm"));

        var step = await console.PromptAsync(select, ct);

        return Parse.Double(step);
    }
}
