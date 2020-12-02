using Microsoft.Win32;

namespace UtilityBox.App.Server.Models.WindowsToggles
{
    public class ClockSeconds : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Show Seconds in taskbar clock";
        public string Description => "Makes the clock in windows task bar show seconds too";
        public int? CheckedValue => 1;
        public int? UncheckedValue => 0;
        public string Key => "ShowSecondsInSystemClock";
        public string RegistryPath => @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKCU:\";
        public bool RequireRestartExplorer => true;
        public bool Available => true;
        public string Message => "Should automatically take change.";
    }
}