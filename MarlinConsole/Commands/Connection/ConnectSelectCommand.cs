using MarlinConsole.Commands.Models;
using MarlinConsole.Models;

namespace MarlinConsole.Commands.Connection;

[Mark(Register.Singleton)]
public class ConnectSelectCommand(ConnectMenu connectMenu) : IAsyncFunction<ConnectModel?>
{
    public async Task<ConnectModel?> ExecuteAsync(CancellationToken ct)
    {
        var device = await connectMenu.SelectDeviceAsync(ct);
        if (device == null)
            return null;

        if (device == Devices.Tcp)
            // TODO: tcp support
            return null;

        var baudRate = await connectMenu.SelectBaudRateAsync(ct);
        if (!baudRate.HasValue)
            return null;

        return new ConnectModel(device, baudRate.Value);
    }
}
