namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public class State(Serial serial)
{
    public bool IsConnected => serial.Port.IsOpen;
    public string Device => serial.Port.PortName;
    public int BaudRate => serial.Port.BaudRate;
}
