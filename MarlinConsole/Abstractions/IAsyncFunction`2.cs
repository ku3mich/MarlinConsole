namespace MarlinConsole.Abstractions;

public interface IAsyncFunction<in TP, TR>
{
    Task<TR> ExecuteAsync(TP parameter, CancellationToken ct);
}
