using Spectre.Console;

namespace Spectre.Selection;

public class SimpleSelectPrompt : IPrompt<string>
{
    public SimpleSelectView View { get; } = new SimpleSelectView();
    public string Show(IAnsiConsole console)
        => ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();

    public SimpleSelectPrompt()
    {
    }

    public SimpleSelectPrompt Options(IEnumerable<string> keys) => Options(keys.ToArray());
    public SimpleSelectPrompt Options(params string[] keys) => Options(keys.Select(s => (s, s)).ToArray());

    public SimpleSelectPrompt AddOptions(params (string key, string value)[] options)
    {
        foreach (var (key, val) in options)
            View.Options.Add(key, val);

        return this;
    }

    public SimpleSelectPrompt Options(params (string key, string value)[] options)
    {
        View.Options.Clear();
        AddOptions(options);
        return this;
    }

    public async Task<string> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        var live = console.Live(View);

        int? result = View.ActiveIndex;

        async Task liveRefresh(LiveDisplayContext ctx)
        {
            ctx.Refresh();

            while (!cancellationToken.IsCancellationRequested)
            {
                var k = await console.Input.ReadKeyAsync(true, cancellationToken);
                if (k == null)
                    continue;

                var key = k.Value.Key;

                if (key == ConsoleKey.Home)
                    View.ActiveIndex = 0;

                if (key == ConsoleKey.End)
                    View.ActiveIndex = View.Options.Count - 1;

                if (key == ConsoleKey.UpArrow)
                {
                    View.ActiveIndex--;
                    if (View.ActiveIndex < 0)
                        View.ActiveIndex = View.Options.Count - 1;
                }

                if (key == ConsoleKey.DownArrow)
                {
                    View.ActiveIndex++;
                    if (View.ActiveIndex >= View.Options.Count)
                        View.ActiveIndex = 0;
                }

                ctx.Refresh();

                if (key == ConsoleKey.Enter || key == ConsoleKey.Spacebar)
                {
                    result = View.ActiveIndex;
                    break;
                }

                if (key == ConsoleKey.Escape)
                {
                    result = null;
                    break;
                }
            }
        }

        await live.StartAsync(liveRefresh);

        if (cancellationToken.IsCancellationRequested)
            result = null;

        if (result == null)
            return null;

        return View.Options.Keys.ElementAt(result.Value);
    }
}
