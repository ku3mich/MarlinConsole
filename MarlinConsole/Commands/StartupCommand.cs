using MarlinConsole.Commands.Connection;
using MarlinConsole.Models;
using Microsoft.Extensions.Options;

namespace MarlinConsole.Commands;

[Mark(By.Register, Injects.Singleton)]
public class StartupCommand(INote note, IOptions<Preferences> options, IOptions<History> history, SerialConnectCommand connect) : IAsyncCommand
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        note.Debug("restoring connection...");
        if (options.Value.IsConnected)
        {
            var model = options.Value.Connect;
            await connect.ExecuteAsync(model, ct);
        }

        note.Debug("restoring history...");
        ReadLine.AddHistory(history.Value.Lines);
    }
}
