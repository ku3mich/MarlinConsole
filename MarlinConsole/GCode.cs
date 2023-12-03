using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace MarlinConsole;

[Mark(By.Register, Injects.Singleton)]
public partial class GCode(Serial serial, INote note)
{
    static string GetGCodeCheckSum(string gcode)
    {
        byte sum = 0;
        foreach (var n in Encoding.UTF8.GetBytes(gcode))
            sum ^= n;

        return $"{sum}";
    }

    static string GetSafeGCode(int n, string gcode)
    {
        var ngcode = $"N{n} {gcode}";
        var sum = GetGCodeCheckSum(ngcode);
        return $"{ngcode}*{sum}";
    }

    public string? StripOK(string? answer) =>
        answer == null ? null : StripOKRegex().Replace(answer, "");

    public string? StripEcho(string? answer) =>
        answer == null ? null : StripEchoRegex().Replace(answer, "");

    public string? Colorize(string? answer)
    {
        if (answer == null)
            return null;

        answer = answer.Replace("[", "[[").Replace("]", "]]");
        answer = CommentsReplaceRegex().Replace(answer, "[gray]$1[/]");
        
        return answer;
    }

    [GeneratedRegex("(;.*)$", RegexOptions.Multiline)]
    private static partial Regex CommentsReplaceRegex();

    [GeneratedRegex("ok\n$", RegexOptions.Multiline)]
    private static partial Regex StripOKRegex();

    [GeneratedRegex("^echo:[ ]*", RegexOptions.Multiline)]
    private static partial Regex StripEchoRegex();

    [GeneratedRegex("^[A-Z]")]
    private static partial Regex IsGCodeLikeRegex();

    public Task<string?> ExecuteAsync(string cmd, int timeout, CancellationToken token)
        => ExecuteAsync(serial.Port, cmd, timeout, token);

    public async Task<string?> ExecuteAsync(SerialPort port, string cmd, int timeoutms, CancellationToken token)
    {
        note.Trace($"executing gcode: [[{cmd}]]");
        if (!port.IsOpen)
        {
            note.Warn("port is not open");
            return null;
        }

        var sb = new StringBuilder();
        var got = false;

        void LocalDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var line = port.ReadExisting();
            sb.Append(line);

            if (line.Length >= 3 && line[^3..] == "ok\n")
                got = true;
        }

        try
        {
            port.DataReceived += LocalDataReceived;
            serial.DisableEcho();

            port.WriteLine(cmd);

            got = false;
            int gotTries = timeoutms / 50 + 1;
            while (!got && gotTries-- > 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(50, token);
            }
        }
        finally
        {
            serial.EnableEcho();
            port.DataReceived -= LocalDataReceived;
        }

        var result = sb.ToString();
        return result;
    }

    public bool IsLikeGCode(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return false;

        return IsGCodeLikeRegex().IsMatch(command);
    }
}
