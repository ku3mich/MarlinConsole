using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace MarlinConsole.Infra;

[Mark(Register.Singleton, Register.As, typeof(INote))]
public class Note(IAnsiConsole console, ILogger<Note> logger) : INote
{
    public NoteLevel ConsoleLevel { get; set; }

    public void Trace(string note)
    {
        logger.LogTrace(note);
        if (ConsoleLevel == NoteLevel.Trace)
            console.Markup($"[teal]. {note}[/]\n");
    }

    public void Debug(string note)
    {
        logger.LogDebug(note);
        if (ConsoleLevel <= NoteLevel.Debug)
            console.Markup($"[lime]> {note}[/]\n");
    }

    public void Info(string note)
    {
        logger.LogInformation(note);
        if (ConsoleLevel <= NoteLevel.Info)
            console.Markup($"[olive]. {note}[/]\n");
    }

    public void Warn(string note)
    {
        logger.LogWarning(note);
        if (ConsoleLevel <= NoteLevel.Warn)
            console.Markup($"[yellow]- {note}[/]\n");
    }

    public void Error(string note)
    {
        logger.LogError(note);
        if (ConsoleLevel <= NoteLevel.Error)
            console.Markup($"[red]* {note}[/]\n");
    }

    public void Fatal(string note, Exception ex)
    {
        logger.LogError(ex, note);

        if (ConsoleLevel <= NoteLevel.Fatal)
        {
            console.Markup($"[red]*** {note}[/]\n");
            console.Markup($"[red]*** {ex}[/]\n");
        }
    }
}
