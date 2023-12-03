namespace MarlinConsole.Commands.Connection;

[Mark(By.Register, Injects.Singleton)]
public class SelectConnectCommand(ConnectSelectCommand select, SerialConnectCommand connect, SaveConnectionStateCommand save) : IAsyncCommand, IHasHelp
{
    public string Help { get; } = "Connects to a communication device";

    public async Task ExecuteAsync(CancellationToken ct)
    {
        var model = await select.ExecuteAsync(ct);
        if (model == null)
            return;

        await connect.ExecuteAsync(model, ct);
        await save.ExecuteAsync(ct);
    }
}
