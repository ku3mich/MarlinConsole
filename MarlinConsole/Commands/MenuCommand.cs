using MarlinConsole.Commands.Axis;
using MarlinConsole.Commands.Settings;

namespace MarlinConsole.Commands;

[Mark(Register.Singleton)]
public class MenuCommand(MenuSelect menuSelect, AsyncCommandInvoker asyncCommandInvoker) : IAsyncCommand, IHasHelp
{
    private static readonly Dictionary<string, Type> Commands = new()
    {
        [Items.Move.AdjustProbe] = typeof(AdjustZProbeCommand),
        [Items.Move.Home] = typeof(MoveHomeCommand),
        [Items.Move.Axis.E] = typeof(MoveAxisECommand),
        [Items.Move.Axis.Z] = typeof(MoveAxisZCommand),
        [Items.Settings.Report] = typeof(SettingsReportCommand),
        [Items.Settings.Speeds] = typeof(SettingsSpeedsCommand),
        [Items.Settings.Save] = typeof(DummyCommand),
    };

    public async Task ExecuteAsync(CancellationToken ct)
    {
        string? item = await menuSelect.Select(ct);
        if (item == null)
            return;

        if (Commands.TryGetValue(item, out Type? cmdType))
        {
            await asyncCommandInvoker.InvokeAsync(cmdType, ct);
        }
        else
        {
            // TODO: to note
        }
    }

    public string Help { get; } = "Shows Menu";
}
