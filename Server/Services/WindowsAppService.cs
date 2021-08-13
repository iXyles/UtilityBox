using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityBox.App.Server.Extensions;
using UtilityBox.App.Server.Models;

namespace UtilityBox.App.Server.Services
{
    public class WindowsAppService
    {
        private const string WindowsPowerShellModule = "Import-Module -Name Appx -UseWindowsPowerShell";
        private readonly PowerShellService _shellService;

        // gotta manually initialize then because electron does not like "reflection"
        public List<IWindowsApp> Apps { get; }
        
        public WindowsAppService(PowerShellService shellService)
        {
            _shellService = shellService;

            var list = new List<IWindowsApp>();
            var type = typeof(IWindowsApp);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName != null && a.FullName.ToLower().Contains("utilitybox"))
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(t => t.IsClass);

            types.ForEach(t => list.Add((IWindowsApp) Activator.CreateInstance(t)));
            Apps = list;
        }

        public async Task<bool> IsAppInstalled(IWindowsApp app) 
            => (await _shellService.RunScriptAsync(
                new List<string>
                {
                    WindowsPowerShellModule,
                    $"Get-AppxPackage *{app.Name}*"
                })).IndexOf(app.Name, StringComparison.OrdinalIgnoreCase) >= 0;

        public async Task UninstallApp(IWindowsApp app)
            => await _shellService.RunScriptAsync(
                new List<string>
                {
                    WindowsPowerShellModule,
                    $"Get-AppxPackage *{app.Name}* | Remove-AppxPackage"
                });

        public async Task<List<AppState>> CheckInstalledApps()
        {
            var apps = new List<AppState>();
            var tasks = Apps.Select(async app =>
            {
                var installed = await IsAppInstalled(app);
                apps.Add(new AppState
                {
                    Name = app.DisplayName,
                    Desc = app.Description,
                    Installed = installed
                });
            });
            await Task.WhenAll(tasks);
            return apps;
        }
    }
}