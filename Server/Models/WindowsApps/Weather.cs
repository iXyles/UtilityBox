namespace UtilityBox.App.Server.Models.WindowsApps
{
    public class Weather : IWindowsApp
    {
        public string Name => "bingweather";

        public string DisplayName => "Weather";

        public string Description => "Windows pre-installed Bing Weather application";
    }
}
