using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.RegistryToggles
{
    public class WindowsCortana : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Cortana";
        public string Description => "By having this set to 'false' windows built in Cortana will be inactive";
        public int? CheckedValue => 1;
        public int? UncheckedValue => 0;
        public string Key => "AllowCortana";
        public string RegistryPath => @"Software\Policies\Microsoft\Windows\Windows Search";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKLM:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(0);
        public string Message => "Requires a restart or log out & in to take affect.";
    }
}