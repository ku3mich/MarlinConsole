using Spectre.Console;

namespace MarlinConsole.Extensions
{
    public static class SpectreConsoleExtensions
    {
        public static Task<TResult> PromptAsync<TPrompt, TResult>(this IAnsiConsole console, CancellationToken ct, Action<TPrompt> promptSetup)
            where TPrompt : IPrompt<TResult>, new()
        {
            var prompt = new TPrompt();
            promptSetup(prompt);
            return prompt.ShowAsync(console, ct);
        }

        public static Task<T> PromptAsync<T>(this IAnsiConsole console, IPrompt<T> prompt, CancellationToken ct) =>
            prompt.ShowAsync(console, ct);

        public static Task StatusAsync(this IAnsiConsole console, string title, CancellationToken ct, Func<StatusContext, CancellationToken, Task> performAction)
            => console.Status().StartAsync(title, async ctx =>
            {
                ctx.Spinner(Spinner.Known.Balloon2);
                await performAction(ctx, ct);
            });

        public static IAnsiConsole Send(this IAnsiConsole console, string? s)
        {
            if (s != null)
                console.Write(s);

            return console;
        }

        public static IAnsiConsole SendMarkup(this IAnsiConsole console, string? s)
        {
            if (s != null)
                console.Markup(s);

            return console;
        }

    }
}