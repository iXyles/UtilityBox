using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityBox.App.Server.Models;
using UtilityBox.Runner.Extensions;

namespace UtilityBox.App.Server.Services
{
    public class WindowsAppService
    {
        private readonly PowerShellService _shellService;

        // gotta manually initialize then because electron does not like "reflection"
        private readonly List<IWindowsApp> _apps;
        
        public WindowsAppService(PowerShellService shellService)
        {
            _shellService = shellService;

            var list = new List<IWindowsApp>();
            var type = typeof(IWindowsApp);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(t => t.IsClass);

            types.ForEach(t => list.Add((IWindowsApp) Activator.CreateInstance(t)));
            _apps = list;
        }

        private async Task<bool> IsAppInstalled(IWindowsApp app) 
            => (await _shellService.RunScriptAsync(
                new List<string>()
                {
                    "Import-Module -Name Appx -UseWIndowsPowershell",
                    $"Get-AppxPackage *{app.Name}*"
                })).IndexOf(app.Name, StringComparison.OrdinalIgnoreCase) >= 0;

        private async Task UninstallApp(IWindowsApp app)
            => await _shellService.RunScriptAsync(
                new List<string>()
                {
                    "Import-Module -Name Appx -UseWIndowsPowershell",
                    $"Get-AppxPackage *{app.Name}* | Remove-AppxPackage"
                });

        private Task InstallApp(IWindowsApp app) => throw new NotImplementedException();

        public async Task<List<AppState>> CheckInstalledApps()
        {
            var apps = new List<AppState>();
            await _apps.ForEachAsync(async app =>
            {
                var installed = await IsAppInstalled(app);
                apps.Add(new AppState
                {
                    Name = app.DisplayName,
                    Desc = app.Description,
                    Installed = installed
                });
            });
            return apps;
        }
    }
}