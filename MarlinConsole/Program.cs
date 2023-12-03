global using MarlinConsole.Abstractions;
global using MarlinConsole.Extensions;
global using MarlinConsole.Infra;
global using Marks.Annotations;

using MarlinConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;
using Nogic.WritableOptions;
using Spectre.Console;
using Extensions.Marks.DependencyInjection;

namespace MarlinConsole;

partial class Program
{
    private static async Task Main(string[] args)
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var provider = new PhysicalFileProvider(userProfile);

        const string preferencesFile = "marlinconsole.preferences.json";
        const string historyFile = "marlinconsole.history.json";

        await Host.CreateDefaultBuilder(args)
            .UseNLog()
            .ConfigureLogging(c =>
            {
                c.AddNLog();
            })
            .ConfigureAppConfiguration((h, builder) =>
            {
                builder
                    .AddJsonFile(provider, preferencesFile, optional: true, reloadOnChange: false)
                    .AddJsonFile(provider, historyFile, optional: true, reloadOnChange: false);

                builder
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var config = context.Configuration;

                services.ConfigureWritableWithExplicitPath<Preferences>(config.GetSection("Preferences"), userProfile, preferencesFile);
                services.ConfigureWritableWithExplicitPath<History>(config.GetSection("History"), userProfile, historyFile);

                services.AddHostedService<ConsoleHostedService>();
                services.AddSingleton(s => AnsiConsole.Console);
                services.AddMarkedFrom(typeof(Program).Assembly);
            })
            .RunConsoleAsync();
    }
}
