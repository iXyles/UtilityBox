using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.WindowsToggles
{
    public class WindowsGameBar : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Windows GameBar";
        public string Description => "By having this set to 'false' windows game bar will be inactive";
        public int? CheckedValue => 1;
        public int? UncheckedValue => 0;
        public string Key => "AppCaptureEnabled";
        public string RegistryPath => @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKCU:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(15019);
        public string Message => "Requires a restart or log out & in to take affect.";
    }
}