namespace MarlinConsole.Abstractions;

public interface IAsyncCommand
{
    Task ExecuteAsync(CancellationToken ct);
}
