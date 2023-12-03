using System.Text;
using MarlinConsole.Models;
using Spectre.Console;

namespace MarlinConsole.Commands.Settings;

[Mark(By.Register, Injects.Singleton)]
public class SettingsSpeedsCommand(IAnsiConsole console, GCode gcode) : IAsyncCommand, IHasHelp
{
    public Task ExecuteAsync(CancellationToken ct) =>
         console.StatusAsync("Reading...", ct,
            async (ctx, ct) =>
            {
                var sb = new StringBuilder();

                async Task Add(string comment, string g)
                {
                    sb.AppendLine(comment);
                    string? result = await gcode.ExecuteAsync(g, Timeouts.Small, ct);
                    if (result != null)
                        sb.Append(gcode.StripOK(result));
                }

                await Add("; Steps per unit:", "M92");
                await Add("; Max feedrates (units/s):", "M203");
                await Add("; Max Acceleration (units/s2):", "M201");
                await Add("; Acceleration (units/s2) (P<print-accel> R<retract-accel> T<travel-accel>):", "M204");
                await Add("; Advanced (B<min_segment_time_us> S<min_feedrate> T<min_travel_feedrate> X<max_jerk> Y<max_jerk> Z<max_jerk> E<max_jerk>):", "M205");

                console.SendMarkup(gcode.Colorize(sb.ToString()));
            });

    public string Help { get; } = "Reports speeds";
}
