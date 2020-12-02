using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.WindowsToggles
{
    public class VolumeMixer : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Old Volume Mixer";
        public string Description => "Change to the old windows Volume mixer";
        public int? CheckedValue => 0;
        public int? UncheckedValue => null;
        public string Key => "EnableMtcUvc";
        public string RegistryPath => @"Software\Microsoft\Windows NT\CurrentVersion\MTCUVC";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKLM:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(0);
        public string Message => "Should automatically take change.";
    }
}