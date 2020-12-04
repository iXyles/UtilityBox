using System.ServiceProcess;
using System.Threading.Tasks;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.Models
{
    public interface IServiceToggle
    {
        public string Name { get; } // The "public name" to show on the view
        public string Description { get; }
        public bool Enabled { get; set; }
        public bool Available { get; }
        public bool InProgress { get; set; }
        public Task Enable(PowerShellService service);
        public Task Disable(PowerShellService service);
        public Task<bool> GetServiceStatus(ServiceController[] services);
    }
}