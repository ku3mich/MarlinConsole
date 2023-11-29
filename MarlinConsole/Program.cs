global using MarlinConsole.Abstractions;
global using MarlinConsole.Extensions;
global using MarlinConsole.Infra;
using MarlinConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;
using Nogic.WritableOptions;
using Spectre.Console;

namespace MarlinConsole;

partial class Program
{
    private static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .UseNLog()
            .ConfigureLogging(c =>
            {
                c.AddNLog();
            })
            .ConfigureAppConfiguration((h, builder) =>
            {
                builder
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("preferences.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("history.json", optional: true, reloadOnChange: false)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var config = context.Configuration;

                services.ConfigureWritable<Preferences>(config.GetSection("Preferences"), file: "preferences.json");
                services.ConfigureWritable<History>(config.GetSection("History"), file: "history.json");

                services.AddHostedService<ConsoleHostedService>();
                services.AddSingleton(s => AnsiConsole.Console);
                services.AddMarkedFrom(typeof(Program).Assembly);
            })
            .RunConsoleAsync();
    }
}
