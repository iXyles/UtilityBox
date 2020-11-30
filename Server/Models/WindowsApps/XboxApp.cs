namespace UtilityBox.App.Server.Models.WindowsApps
{
    public class XboxApp : IWindowsApp
    {
        public string Name => "xboxapp";

        public string DisplayName => "Xbox App";

        public string Description => "Windows pre-installed Xbox app (associated to all apps connected to Xbox App!)";
    }
}
