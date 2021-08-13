using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.Models.ServiceToggles
{
    public class WindowsSearchService : IServiceToggle
    {
        public string Name => "Windows Search";

        public string Description =>
            "Windows indexing service of files & applications - Can be disk heavy for HDD, required for Outlook search to work.";
        public bool Enabled { get; set; }
        public bool Available => _service != null;
        public bool InProgress { get; set; }
        private ServiceController _service;
        
        public async Task Enable(PowerShellService service)
        {
            await service.RunScriptAsync(new List<string>
            {
                "Set-Service -Name WSearch -StartupType Automatic",
                "Start-Service -Name WSearch"
            });
            Enabled = true;
        }

        public async Task Disable(PowerShellService service)
        {
            await service.RunScriptAsync(new List<string>
            {
                "Set-Service -Name WSearch -StartupType Disabled",
                "Stop-Service -Name WSearch"
            });
            Enabled = false;
        }

        public Task<bool> GetServiceStatus(ServiceController[] services)
        {
            _service = services.FirstOrDefault(s => s.ServiceName.Equals("WSearch", StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(_service != null && _service.StartType != ServiceStartMode.Disabled);
        }
    }
}