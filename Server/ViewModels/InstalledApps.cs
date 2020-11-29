using System.Threading.Tasks;
using DotNetify;
using UtilityBox.App.Server.Models;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.ViewModels
{
    public class InstalledApps : BaseVM
    {
        private AppState[] _installedApps = new AppState[0];
        public AppState[] InstalledWindowsApps => _installedApps;

        public InstalledApps(WindowsAppService appService)
        {
            Task.Run(async () =>
            {
                _installedApps = (await appService.CheckInstalledApps()).ToArray();
                Changed(nameof(InstalledWindowsApps));
                PushUpdates();
            });
        }
    }
}