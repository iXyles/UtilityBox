using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using DotNetify;
using UtilityBox.App.Server.Extensions;
using UtilityBox.App.Server.Models;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.ViewModels
{
    public class ServiceToggles : MulticastVM
    {
        private static PowerShellService _powerShellService;
        private static readonly List<IServiceToggle> Services = new();
        public IServiceToggle[] AvailableToggles => Services.Where(x => x.Available).ToArray();

        public ServiceToggles(PowerShellService powerShellService)
        {
            if (_powerShellService == null)
            {
                _powerShellService = powerShellService;
                LoadServices(); 
            }
        }

        public Action RefreshServices => () =>
        {
            // clear services and push to view (forces the view to do "loading...")
            Services.Clear();
            Changed(nameof(AvailableToggles));
            PushUpdates();

            // get updated list and push to view
            LoadServices();
        };

        private void LoadServices() =>
            Task.Run(async () =>
            {
                var type = typeof(IServiceToggle);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p))
                    .Where(t => t.IsClass);
                types.ForEach(t => Services.Add((IServiceToggle) Activator.CreateInstance(t)));
                var services = ServiceController.GetServices();
                await Services.ForEachAsync(async x => x.Enabled = await x.GetServiceStatus(services));
                Changed(nameof(AvailableToggles));
                PushUpdates();
            });
        
        public Action<string> Toggle => key =>
            Task.Run(async () =>
            {
                var service = Services.First(x => x.Name == key);
                await ToggleService(service);
                Changed(nameof(AvailableToggles));
                PushUpdates();
            });

        void UpdateProgressStatus(IServiceToggle service, bool status)
        {
            service.InProgress = status;
            Changed(nameof(AvailableToggles));
            PushUpdates();
        }
        
        async Task ToggleService(IServiceToggle service)
        {
            if (!service.Available) return;
            
            UpdateProgressStatus(service, true);
            if (service.Enabled)
                await service.Disable(_powerShellService);
            else
                await service.Enable(_powerShellService);
            UpdateProgressStatus(service, false);
            Changed(nameof(AvailableToggles));
            PushUpdates();
        }
    }
}