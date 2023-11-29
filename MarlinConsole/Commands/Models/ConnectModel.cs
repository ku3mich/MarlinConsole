namespace MarlinConsole.Commands.Models
{
    public class ConnectModel(string? device, int baudRate)
    {
        public string? Device { get; set; } = device;
        public int BaudRate { get; set; } = baudRate;

        public ConnectModel() : this(null, 0) { }
    }
}
