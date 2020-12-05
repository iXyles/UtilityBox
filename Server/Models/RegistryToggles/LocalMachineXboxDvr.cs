using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.RegistryToggles
{
    public class LocalMachineXboxDvr : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Machine Xbox DVR";
        public string Description => "Enable Windows Xbox DVR for the whole machine.";
        public int? CheckedValue => null;
        public int? UncheckedValue => 0;
        public string Key => "AllowGameDVR";
        public string RegistryPath => @"SOFTWARE\Policies\Microsoft\Windows\GameDVR";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKLM:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(0);
        public string Message => "Requires a restart or log out & in to take affect.";
    }
}