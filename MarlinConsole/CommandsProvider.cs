using MarlinConsole.Commands;
using MarlinConsole.Commands.Axis;
using MarlinConsole.Commands.Connection;

namespace MarlinConsole;

[Mark(Register.Singleton)]
public class CommandsProvider
{
    public readonly Dictionary<string, Type> Commands = new()
    {
        ["/?"] = typeof(HelpCommand),
        ["/m"] = typeof(MenuCommand),
        ["/c"] = typeof(SelectConnectCommand),
        ["/s"] = typeof(StateCommand),
        ["/l"] = typeof(ClearCommand),
        ["/mz"] = typeof(MoveAxisZCommand),
        ["/me"] = typeof(MoveAxisECommand)
    };
}
