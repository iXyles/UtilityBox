using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.RegistryToggles
{
    public class WindowsUpdateShare : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Disable Windows Update Share";
        public string Description => "Disable sharing windows updates (p2p)";
        public int? CheckedValue => 0;
        public int? UncheckedValue => 3; // windows default share with internet
        public string Key => "DODownloadMode";
        public string RegistryPath => @"SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKLM:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(0);
        public string Message => "Requires a restart or log out & in to take affect";
    }
}