namespace MarlinConsole.Infra;

public static class NoteExtensions
{
    public static void Fatal(this INote note, Exception ex)
    {
        note.Fatal("Fatal", ex);
    }
}