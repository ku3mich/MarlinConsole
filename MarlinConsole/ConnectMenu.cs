using System.IO.Ports;
using MarlinConsole.Models;
using Spectre.Console;
using Spectre.Selection;

namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public class ConnectMenu(IAnsiConsole console)
{
    public async Task<int?> SelectBaudRateAsync(CancellationToken ct)
    {
        var prompt = new SimpleSelectPrompt()
            .Options(
                "250000",
                "230400",
                "115200",
                "57600",
                "38400",
                "19200",
                "9600");

        var rate = await console.PromptAsync(prompt, ct);
        return Parse.Int(rate);
    }

    public async Task<string?> SelectDeviceAsync(CancellationToken ct)
    {
        var ports = SerialPort.GetPortNames();

        var port = await console
            .PromptAsync<SimpleSelectPrompt, string?>(ct,
                p => p.Options(ports).AddOptions((Devices.Tcp, "TCP Port")));

        return port;
    }
}
