using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.RegistryToggles
{
    public class UserXboxDvr : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "User Xbox DVR";
        public string Description => "Windows Xbox DVR for the current user";
        public int? CheckedValue => null;
        public int? UncheckedValue => 0;
        public string Key => "GameDVR_Enabled";
        public string RegistryPath => @"System\GameConfigStore";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKCU:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(0);
        public string Message => "Requires a restart or log out & in to take affect.";
    }
}