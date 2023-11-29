namespace MarlinConsole.Infra;

public interface INote
{
    NoteLevel ConsoleLevel { get; set; }

    void Trace(string note);
    void Debug(string note);
    void Info(string note);
    void Warn(string note);
    void Error(string note);
    void Fatal(string note, Exception ex);
}
