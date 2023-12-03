using MarlinConsole.Commands.Models;
using MarlinConsole.Models;
using Nogic.WritableOptions;

namespace MarlinConsole.Commands.Connection;

[Mark(By.Register, Injects.Singleton)]
public class SaveConnectionStateCommand(State state, INote note, IWritableOptions<Preferences> options) : IAsyncCommand
{
    public Task ExecuteAsync(CancellationToken ct)
    {
        options.Update(new Preferences
        {
            Connect = new ConnectModel
            {
                BaudRate = state.BaudRate,
                Device = state.Device,
            },
            IsConnected = state.IsConnected
        }, reload: true);

        note.Info("Preferences updated");

        return Task.CompletedTask;
    }
}
