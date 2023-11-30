namespace MarlinConsole.Extensions
{
    public static class Parse
    {
        public static double? Double(string? num) => string.IsNullOrWhiteSpace(num) ? null : double.Parse(num);
        public static int? Int(string? num) => string.IsNullOrWhiteSpace(num) ? null : int.Parse(num);
    }
}
