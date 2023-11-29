using System.IO.Ports;
using Spectre.Console;
namespace MarlinConsole;

[Mark(Register.Singleton)]
public class Serial : IDisposable
{
    public readonly SerialPort Port = new();
    private readonly IAnsiConsole console;
    
    private bool disposedValue;
    private bool EchoEnabled = true;

    public Serial(IAnsiConsole console)
    {
        Port.DataReceived += (s, e) =>
        {
            if (!EchoEnabled)
                return;

            var data = Port.ReadExisting();
            console.Write(data);
        };
        this.console = console;
    }

    void Connect(string device, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    {
        if (Port.IsOpen)
            Port.Close();

        Port.PortName = device;
        Port.StopBits = stopBits;
        Port.DataBits = dataBits;
        Port.Parity = parity;
        Port.BaudRate = baudRate;
        Port.RtsEnable = true;
        Port.DtrEnable = true;
        Port.Handshake = Handshake.None;
        Port.ReadTimeout = 500;
        Port.WriteTimeout = 500;
        
        Port.Open();
    }

    public void Connect(string device, int baudRate)
    {
        Connect(device, baudRate, Parity.None, 8, StopBits.One);
    }

    public void Disconnect()
    {
        if (Port.IsOpen)
            Port.Close();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Port.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void DisableEcho() => EchoEnabled = false;

    public void EnableEcho() => EchoEnabled = true;
}
