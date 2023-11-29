using MarlinConsole.Commands.Models;

namespace MarlinConsole.Models;

public class Preferences
{
    public ConnectModel Connect { get; set; } = new();
    public bool IsConnected { get; set; }
}
