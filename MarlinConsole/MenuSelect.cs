namespace MarlinConsole;

[Mark(Register.Singleton)]
public class MenuSelect(Select select)
{
    static readonly Dictionary<string, Func<Select, Func<string?>>> Menus = new()
    {
        [Items.Move.Menu] = s => s.Move,
        [Items.Move.AxisMenu] = s => s.AxisMove,
        [Items.Settings.Menu] = s => s.Settings
    };

    public Task<string?> Select(CancellationToken ct)
    {
        string? item = null;
        Stack<Func<string?>> menuStack = new();
        menuStack.Push(select.Menu);

        while (!ct.IsCancellationRequested && menuStack.Count > 0)
        {
            var menu = menuStack.Pop();
            item = menu();
            if (item == null)
                continue;

            Func<string?>? findMenu(string item) =>
                Menus.TryGetValue(item, out Func<Select, Func<string?>>? value) ? value(select) : null;

            var nextMenu = findMenu(item);

            if (nextMenu != null)
            {
                menuStack.Push(menu);
                menuStack.Push(nextMenu);
                continue;
            }

            break;
        }

        return Task.FromResult(item);
    }
}
