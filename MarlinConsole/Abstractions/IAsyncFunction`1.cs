namespace MarlinConsole.Abstractions;

public interface IAsyncFunction<TR>
{
    Task<TR> ExecuteAsync(CancellationToken ct);
}
