namespace UtilityBox.App.Server.Models.WindowsApps
{
    public class People : IWindowsApp
    {
        public string Name => "people";

        public string DisplayName => "People";

        public string Description => "Windows pre-installed People application";
    }
}
