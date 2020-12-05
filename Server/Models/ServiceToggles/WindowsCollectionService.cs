using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.Models.ServiceToggles
{
    public class WindowsCollectionService : IServiceToggle
    {
        public string Name => "Windows collection (Telemetry)";
        public string Description => "Allow windows to be collection data of your system and applications (does not apply to third-party apps)";
        public bool Enabled { get; set; }
        public bool Available => _services.Count == 2;
        public bool InProgress { get; set; }
        private List<ServiceController> _services = new();
        private const string RegistryPath = @"HKLM:\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
        private const string RegistryKey = "AllowTelemetry";
        
        public async Task Enable(PowerShellService service)
        {
            await service.RunScriptAsync(new List<string>
            {
                "Set-Service -Name DiagTrack -StartupType Automatic",
                "Start-Service -Name DiagTrack",
                "Set-Service -Name dmwappushservice -StartupType Automatic",
                "Start-Service -Name dmwappushservice"
            });
            await SetTelemetryValue(service, 3);
            Enabled = true;
        }

        public async Task Disable(PowerShellService service)
        {
            await service.RunScriptAsync(new List<string>
            {
                "Set-Service -Name DiagTrack -StartupType Disabled",
                "Stop-Service -Name DiagTrack",
                "Set-Service -Name dmwappushservice -StartupType Disabled",
                "Stop-Service -Name dmwappushservice"
            });
            await SetTelemetryValue(service, 0);
            Enabled = false;
        }

        private async Task SetTelemetryValue(PowerShellService service, int value)
        {
            var queries = new List<string>
            {
                $"New-Item -Path \"{RegistryPath}\" -Force | Out-Null",
                $"New-ItemProperty -Path \"{RegistryPath}\" -Name \"{RegistryKey}\" -Value {value} -PropertyType DWORD -Force | Out-Null",
            };
            await service.RunScriptAsync(queries);
        }

        public Task<bool> GetServiceStatus(ServiceController[] services)
        {
            _services = services.Where(s => s.ServiceName.Equals("DiagTrack", StringComparison.OrdinalIgnoreCase)
                || s.ServiceName.Equals("dmwappushservice", StringComparison.OrdinalIgnoreCase)).ToList();
            return Task.FromResult(_services.Count == 2 && _services.All(x => x.StartType != ServiceStartMode.Disabled));
        }
    }
}