namespace MarlinConsole;

[Serializable]
internal class ConsoleException : Exception
{
    public ConsoleException()
    {
    }

    public ConsoleException(string? message) : base(message)
    {
    }

    public ConsoleException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}