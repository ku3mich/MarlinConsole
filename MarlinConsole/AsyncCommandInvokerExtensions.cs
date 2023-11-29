namespace MarlinConsole;

public static class AsyncCommandInvokerExtensions
{
    public static Task InvokeAsync<T>(this AsyncCommandInvoker invoker, CancellationToken ct) where T : IAsyncCommand
        => invoker.InvokeAsync(typeof(T), ct);
}