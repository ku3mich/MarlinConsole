using Spectre.Console;

namespace MarlinConsole.Commands;

[Mark(Register.Singleton)]
public class DummyCommand(IAnsiConsole console, INote note) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct)
    {
        console.WriteLine("dummy: not implemented");

        console.Markup("text [grey]text example[/] text\n");
        console.Markup("text [silver]text example[/] text\n");
        console.Markup("text [olive]text example[/] text\n");
        console.Markup("text [red]text example[/] text\n");
        console.Markup("text [lime]text example[/] text\n");
        console.Markup("text [yellow]text example[/] text\n");
        console.Markup("text [teal]text example[/] text\n");

        note.Trace("trace");
        note.Debug("debug");
        note.Info("info");
        note.Warn("warn");
        note.Error("error");

        try
        {
            throw new IOException("oops");
        }
        catch (Exception ex)
        {
            note.Fatal("fatal", ex);
        }

        return Task.CompletedTask;
    }

    public string Help { get; } = "dummy: not implemented";
}
