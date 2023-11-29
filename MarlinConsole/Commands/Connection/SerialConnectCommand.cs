using MarlinConsole.Commands.Models;

namespace MarlinConsole.Commands.Connection;

[Mark(Register.Singleton)]
public class SerialConnectCommand(Serial serial, Manager manager, INote note) : IAsyncAction<ConnectModel>
{
    public Task ExecuteAsync(ConnectModel parameter, CancellationToken ct)
    {
        manager.Preferences.IsConnected = false;
        try
        {
            note.Info($"Connecting to [[{parameter.Device!}]]/{parameter.BaudRate}...");
            serial.Connect(parameter.Device!, parameter.BaudRate);
            manager.Preferences.IsConnected = true;
            note.Info("...Connected");
        }
        catch (UnauthorizedAccessException ex)
        {
            note.Fatal(ex);
        }
        catch (IOException ex)
        {
            note.Fatal(ex);
        }

        return Task.CompletedTask;
    }
}
