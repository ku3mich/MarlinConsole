using Spectre.Console;

namespace Spectre.Spinner;

public class SpinnerPrompt : IPrompt<double?>
{
    private Func<double, double> OnChangeHandler = s => s;

    public double? Show(IAnsiConsole console)
        => ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();

    protected SpinnerView View { get; } = new SpinnerView(2, 0.1, 0, double.MaxValue);

    public SpinnerPrompt Prompt(string prompt, Style promptStyle)
    {
        View.Prompt = prompt;
        View.PromptStyle = promptStyle;
        return this;
    }

    public SpinnerPrompt Prompt(string prompt) => Prompt(prompt, Style.Plain);

    public SpinnerPrompt Value(double value, Style valueStyle)
    {
        View.ValueStyle = valueStyle;
        View.Value = value;
        return this;
    }

    public SpinnerPrompt Value(double value) => Value(value, Style.Plain);

    public SpinnerPrompt()
    {
    }

    public SpinnerPrompt OnChange(Func<double, double> onChange)
    {
        OnChangeHandler = onChange;
        return this;
    }

    public SpinnerPrompt OnChangeAsync(Func<double, Task<double>> onChange)
    {
        OnChangeHandler = e =>
            onChange(e).GetAwaiter().GetResult();

        return this;
    }

    public SpinnerPrompt Max(double max)
    {
        View.Max = max;
        return this;
    }

    public SpinnerPrompt Min(double min)
    {
        View.Min = min;
        return this;
    }

    public SpinnerPrompt Step(double step)
    {
        View.Step = step;
        return this;
    }

    public async Task<double?> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        var live = console.Live(View);

        double? result = View.Value;

        async Task liveRefresh(LiveDisplayContext ctx)
        {
            ctx.Refresh();

            while (!cancellationToken.IsCancellationRequested)
            {
                var k = await console.Input.ReadKeyAsync(true, cancellationToken);
                if (k == null)
                    continue;

                var key = k.Value.Key;

                if (key == ConsoleKey.UpArrow)
                {
                    View.Increase();
                    View.Value = OnChangeHandler(View.Value);
                }

                if (key == ConsoleKey.DownArrow)
                {
                    View.Decrease();
                    View.Value = OnChangeHandler(View.Value);
                }

                ctx.Refresh();

                if (key == ConsoleKey.Enter || key == ConsoleKey.Spacebar)
                {
                    result = View.Value;
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

        return result;
    }
}
