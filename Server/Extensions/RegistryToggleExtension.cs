using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityBox.App.Server.Models;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.Extensions
{
    public static class RegistryToggleExtension
    {
        public static async Task<bool> IsActive(this IRegistryToggle toggle, PowerShellService powerShellService)
        {
            try
            {
                var query = $"Get-ItemProperty \"{toggle.RegistryEntry}{toggle.RegistryPath}\" | SELECT \"{toggle.Key}\"";
                var pattern = new Regex($"@{{{toggle.Key}=(?<value>\\d+)}}");
                var result = await powerShellService.RunScriptAsync(query);
                var match = pattern.Match(result);
                var value = match.Groups.Count > 0 ? match.Groups["value"].Value : string.Empty;
                
                if (string.IsNullOrEmpty(result) && !toggle.CheckedValue.HasValue)
                    return true;
                if (string.IsNullOrEmpty(result) && !toggle.UncheckedValue.HasValue)
                    return false;
                
                return toggle.CheckedValue.HasValue
                    ? value == toggle.CheckedValue.Value.ToString()
                    : string.IsNullOrEmpty(value);
            } catch { /* ignored */ }
            return false;
        }
    }
}