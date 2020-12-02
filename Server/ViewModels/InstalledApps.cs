using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetify;
using UtilityBox.App.Server.Extensions;
using UtilityBox.App.Server.Models;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.ViewModels
{
    public class InstalledApps : MulticastVM
    {
        private static AppState[] _installedApps = Array.Empty<AppState>();
        private readonly WindowsAppService _appService;

        public AppState[] InstalledWindowsApps => _installedApps;
        public bool InProgress { get; private set; }

        public InstalledApps(WindowsAppService appService)
        {
            if (_installedApps.Length == 0 && !InProgress)
                Task.Run(async () =>
                {
                    SendInProgressStatus(true);
                    _installedApps = (await _appService.CheckInstalledApps()).ToArray();
                    Changed(nameof(InstalledWindowsApps));
                    PushUpdates();
                    SendInProgressStatus(false);
                });

            _appService = appService;
        }

        private void SendInProgressStatus(bool status)
        {
            InProgress = status;
            Changed(nameof(InProgress));
            PushUpdates();
        }

        public Action RefreshApps => () => Task.Run(async () =>
        {
            SendInProgressStatus(true);

            // clear apps and push to view (forces the view to do "loading...")
            _installedApps = Array.Empty<AppState>();
            Changed(nameof(InstalledWindowsApps));
            PushUpdates();

            // get updated list and push to view
            _installedApps = (await _appService.CheckInstalledApps()).ToArray();
            Changed(nameof(InstalledWindowsApps));
            PushUpdates();

            SendInProgressStatus(false);
        });

        public Action<string> UninstallApp => app =>
        {
            SendInProgressStatus(true);
            Task.Run(async () =>
            {
                var windowsApp = _appService.Apps.First(x => x.DisplayName.Equals(app));
                await _appService.UninstallApp(windowsApp);
                var installed = await _appService.IsAppInstalled(windowsApp);
                _installedApps.Where(x => x.Name.Equals(app)).ForEach(x => x.Installed = installed);

                InProgress = false;
                Changed(nameof(InProgress));
                Changed(nameof(InstalledWindowsApps));
                PushUpdates();
            });
        };
    }
}