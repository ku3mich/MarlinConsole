﻿global using MarlinConsole.Abstractions;
global using MarlinConsole.Extensions;
global using MarlinConsole.Infra;
using MarlinConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var provider = new PhysicalFileProvider(userProfile);

        await Host.CreateDefaultBuilder(args)
            .UseNLog()
            .ConfigureLogging(c =>
            {
                c.AddNLog();
            })
            .ConfigureAppConfiguration((h, builder) =>
            {
                builder
                    .AddJsonFile(provider, "marlinconsole.preferences.json", optional: true, reloadOnChange: false)
                    .AddJsonFile(provider, "marlinconsole.history.json", optional: true, reloadOnChange: false);

                builder
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var config = context.Configuration;

                services.ConfigureWritableWithExplicitPath<Preferences>(config.GetSection("Preferences"), userProfile, file: "marlinconsole.preferences.json");
                services.ConfigureWritableWithExplicitPath<History>(config.GetSection("History"), userProfile, file: "marlinconsole.history.json");

                services.AddHostedService<ConsoleHostedService>();
                services.AddSingleton(s => AnsiConsole.Console);
                services.AddMarkedFrom(typeof(Program).Assembly);
            })
            .RunConsoleAsync();
    }
}
