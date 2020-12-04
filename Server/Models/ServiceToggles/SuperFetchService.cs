using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using UtilityBox.App.Server.Extensions;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.Models.ServiceToggles
{
    public class SuperFetchService : IServiceToggle
    {
        private ServiceController _service;
        public string Name => "Superfetch";
        public string Description => "Preloading applications you usually use into memory to launch faster.";
        public bool Enabled { get; set; }
        public bool Available => _service != null;
        public bool InProgress { get; set; }

        public async Task Enable(PowerShellService service)
        {
            await service.RunScriptAsync(new List<string>
            {
                "Set-Service -Name SysMain -StartupType Automatic",
                "Start-Service -Name SysMain"
            });
            Enabled = true;
        }

        public async Task Disable(PowerShellService service)
        {
            var result = await service.RunScriptAsync(new List<string>
            {
                "Set-Service -Name SysMain -StartupType Disabled",
                "Stop-Service -Name SysMain -Force -Confirm"
            });
            
            Enabled = false;
        }

        public Task<bool> GetServiceStatus(ServiceController[] services)
        {
            _service = services.FirstOrDefault(s => s.ServiceName.Equals("SysMain", StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(_service != null && _service.StartType != ServiceStartMode.Disabled);
        }
    }
}