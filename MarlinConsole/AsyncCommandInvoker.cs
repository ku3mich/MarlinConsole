using Microsoft.Extensions.DependencyInjection;

namespace MarlinConsole;

[Mark(Register.Transient)]
public class AsyncCommandInvoker
{
    private readonly IServiceProvider provider;

    public AsyncCommandInvoker(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public Task InvokeAsync(Type t, CancellationToken ct)
    {
        var cmd = (IAsyncCommand)provider.GetRequiredService(t);
        return cmd.ExecuteAsync(ct);
    }
}
