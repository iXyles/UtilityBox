using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.RegistryToggles
{
    public class Ndu : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Disable Windows Ndu";
        public string Description => "Memory leak issue? Set this to 'checked' to try disable NDU.";
        public int? CheckedValue => 4;
        public int? UncheckedValue => 2;
        public string Key => "Start";
        public string RegistryPath => @"SYSTEM\ControlSet001\Services\Ndu";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKLM:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(0);
        public string Message => "Requires a restart or log out & in to take affect, try update audio/net drivers before this.";
    }
}