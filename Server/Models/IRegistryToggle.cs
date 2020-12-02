namespace UtilityBox.App.Server.Models
{
    public interface IRegistryToggle
    {
        public bool Active { get; set; }
        public string Name { get; }
        public string Description { get; }
        public int? CheckedValue { get; }
        public int? UncheckedValue { get; }
        public string Key { get; }
        public string RegistryPath { get; }
        public string RegistryValueType { get; }
        public string RegistryEntry { get; }
        public bool RequireRestartExplorer { get; }
        public bool Available { get; }
        public string Message { get; }
    }
}