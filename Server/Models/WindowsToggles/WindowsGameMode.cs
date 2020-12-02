using Microsoft.Win32;
using UtilityBox.App.Server.Extensions;

namespace UtilityBox.App.Server.Models.WindowsToggles
{
    public class WindowsGameMode : IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name => "Windows GameMode";
        public string Description => "By having this set to 'false' windows game mode will be inactive";
        public int? CheckedValue => 1;
        public int? UncheckedValue => 0;
        public string Key => "AllowAutoGameMode";
        public string RegistryPath => @"SOFTWARE\Microsoft\GameBar";
        public string RegistryValueType => nameof(RegistryValueKind.DWord);
        public string RegistryEntry => @"HKCU:\";
        public bool RequireRestartExplorer => false;
        public bool Available => WindowsExtension.IsWindows10(15019);
        public string Message => "Requires a restart or log out & in to take affect.";
    }
}