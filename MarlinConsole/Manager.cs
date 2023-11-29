using MarlinConsole.Models;
using Nogic.WritableOptions;

namespace MarlinConsole;

[Mark(Register.Singleton)]
public class Manager(IWritableOptions<Preferences> options)
{
    public Preferences Preferences { get; } = options.Value;

    public void SavePreferences()
    {
        options.Update(options.Value);
    }
}
