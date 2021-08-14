namespace UtilityBox.App.Server.Models.WindowsApps
{
    public class Store : IWindowsApp
    {
        public string Name => "windowsstore";

        public string DisplayName => "Microsoft Store";

        public string Description => "Windows pre-installed Windows Store";
    }
}
