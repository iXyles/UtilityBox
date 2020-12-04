using Microsoft.Win32;

namespace UtilityBox.App.Server.Models.RegistryToggles
{
    public class AutoUpdate : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Notify Windows Updates";

        public string Description => "If this is set to 'true' windows updates may notify for updates, but all updates become manual.";
        public int? CheckedValue => 2;
        public int? UncheckedValue => 5;
        public string Key => "AUOptions";
        public string RegistryPath => @"Software\Policies\Microsoft\Windows\WindowsUpdate\AU";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKLM:\";
        public bool RequireRestartExplorer => false;
        public bool Available => true;
        public string Message => "Should automatically take change.";
    }
}