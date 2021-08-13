using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetify;
using UtilityBox.App.Server.Extensions;
using UtilityBox.App.Server.Models;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.ViewModels
{
    public class RegistryToggles : BaseVM
    {
        private readonly List<IRegistryToggle> _regToggles = new();
        public List<IRegistryToggle> Toggles => _regToggles.Where(x => x.Available).ToList();
        private readonly UpdateRegistryService _updateService;
        private readonly PowerShellService _powerShellService;

        public RegistryToggles(UpdateRegistryService updateService, PowerShellService powerShellService)
        {
            LoadRegistryKeys();
            _updateService = updateService;
            _powerShellService = powerShellService;
        }

        private Task LoadRegistryKeys() =>
            Task.Run(async () =>
            {
                var type = typeof(IRegistryToggle);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName != null && a.FullName.ToLower().Contains("utilitybox"))
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p))
                    .Where(t => t.IsClass);
                types.ForEach(t => _regToggles.Add((IRegistryToggle) Activator.CreateInstance(t)));
                await _regToggles.ForEachAsync(async x => x.Active = await x.IsActive(_powerShellService));
                Changed(nameof(Toggles));
                PushUpdates();
            });

        public Action<string> Toggle => key =>
            Task.Run(async () =>
            {
                var toggle = _regToggles.First(x => x.Name == key);
                await _updateService.ToggleValue(toggle);
                Changed(nameof(Toggles));
                PushUpdates();
            });

        private bool _refreshing = false;
        public Action RefreshRegistryKeys => () => Task.Run(async () =>
        {
            if (_refreshing) return;
            _refreshing = true;

            // clear toggles and push to view (forces the view to do "loading...")
            _regToggles.Clear();
            Changed(nameof(Toggles));
            PushUpdates();

            // get updated list and push to view
            await LoadRegistryKeys();
            Changed(nameof(Toggles));
            PushUpdates();
            _refreshing = false;
        });
    }
}