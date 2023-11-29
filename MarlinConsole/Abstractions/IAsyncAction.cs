namespace MarlinConsole.Abstractions;

public interface IAsyncAction<T>
{
    Task ExecuteAsync(T parameter, CancellationToken ct);
}
