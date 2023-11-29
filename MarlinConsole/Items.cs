namespace MarlinConsole;

static class Items
{
    public static class Move
    {
        public const string Menu = "move-menu";

        public const string Home = "move-home";
        public const string AdjustProbe = "move-adjust-probe";
        public const string AxisMenu = "move-axis";

        public static class Axis
        {
            public const string Z = "move-axis-z";
            public const string E = "move-axis-e";
        }
    }

    public static class Settings
    {
        public const string Menu = "settings-menu";

        public const string Report = "settings-report";
        public const string Speeds = "settings-speeds";
        public const string Save = "settings-save";
    }
}
