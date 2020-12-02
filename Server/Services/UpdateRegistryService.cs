using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UtilityBox.App.Server.Extensions;
using UtilityBox.App.Server.Models;

namespace UtilityBox.App.Server.Services
{
    public class UpdateRegistryService
    {
        private readonly PowerShellService _powerShellService;

        public UpdateRegistryService(PowerShellService powerShellService)
        {
            _powerShellService = powerShellService;
        }

        public async Task ToggleValue(IRegistryToggle toggle)
        {
            try
            {
                if (toggle.UncheckedValue == null && toggle.Active || 
                    toggle.CheckedValue == null && !toggle.Active)
                    await RemoveRegistryKey(toggle);
                else
                    await UpdateRegistryKey(
                        toggle,
                        toggle.Active
                            ? toggle.UncheckedValue
                            : toggle.CheckedValue);
                
                toggle.Active = await toggle.IsActive(_powerShellService);
                if (!toggle.RequireRestartExplorer)
                    return; //Success

                try
                {
                    Process.GetProcesses().First(p => p.ProcessName.Equals("explorer")).Kill();
                    Process.Start("explorer.exe");
                } catch { /* ignored */ }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Some exception error...
            }
        }

        private async Task RemoveRegistryKey(IRegistryToggle toggle)
        {
            var query = $"Remove-ItemProperty -Path \"{toggle.RegistryEntry}{toggle.RegistryPath}\" -Name \"{toggle.Key}\"";
            await _powerShellService.RunScriptAsync(query);
        }

        private async Task UpdateRegistryKey(IRegistryToggle toggle, int? value)
        {
            if (!value.HasValue)
                return;
            
            var queries = new List<string>
            {
                $"New-Item -Path \"{toggle.RegistryEntry}{toggle.RegistryPath}\" -Force | Out-Null",
                $"New-ItemProperty -Path \"{toggle.RegistryEntry}{toggle.RegistryPath}\" -Name \"{toggle.Key}\" -Value {value.Value} -PropertyType DWORD -Force | Out-Null",
            };
            await _powerShellService.RunScriptAsync(queries);
        }
    }
}