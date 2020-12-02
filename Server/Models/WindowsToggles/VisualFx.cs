using Microsoft.Win32;

namespace UtilityBox.App.Server.Models.WindowsToggles
{
    public class VisualFx : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Best Performance Visual effects";
        public string Description => "Changes the visual effects of windows to pure performance.";
        public int? CheckedValue => 2;
        public int? UncheckedValue => 3;
        public string Key => "VisualFXSetting";
        public string RegistryPath => @"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKCU:\";
        public bool RequireRestartExplorer => false;
        public bool Available => true;
        public string Message => "MAY require a restart or log out & in to take affect.";
    }
}