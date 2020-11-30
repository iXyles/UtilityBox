namespace UtilityBox.App.Server.Models.WindowsApps
{
    public class News : IWindowsApp
    {
        public string Name => "bingnews";

        public string DisplayName => "News";

        public string Description => "Windows pre-installed Bing News application";
    }
}
