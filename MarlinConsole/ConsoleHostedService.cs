using Microsoft.Extensions.Hosting;

namespace MarlinConsole;
internal sealed class ConsoleHostedService(INote note, IHostApplicationLifetime appLifetime, Application application) : IHostedService
{
    private CancellationTokenSource source = new CancellationTokenSource();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        note.Debug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    note.Info("application run...");
                    await application.Run(source.Token);
                }
                catch (Exception ex)
                {
                    note.Fatal(ex);
                }
                finally
                {
                    note.Info("... application done");
                    appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        source.Cancel();
        return Task.CompletedTask;
    }
}