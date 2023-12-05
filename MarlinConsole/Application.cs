using Marks.Annotations;
using MarlinConsole.Commands;
using MarlinConsole.Models;
using Nogic.WritableOptions;

namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public class Application(CommandProcessor processor, StartupCommand startup, IWritableOptions<History> history)
{
    public async Task Run(CancellationToken ct)
    {
        await startup.ExecuteAsync(ct);

        while (!ct.IsCancellationRequested)
        {
            // TODO: async
            var i = ReadLine.Read("# ");

            if (!string.IsNullOrEmpty(i))
            {
                ReadLine.AddHistory(i);
                var lines = ReadLine.GetHistory();

                history.Update(new History
                {
                    Lines = lines.Count > 1024 ? lines.Skip(lines.Count - 1024).ToArray() : lines.ToArray()
                }, false);
            }

            if (i == "/q")
                break;

            await processor.Process(i, ct);
        }
    }
}
