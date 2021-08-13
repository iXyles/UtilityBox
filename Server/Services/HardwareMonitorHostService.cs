using System.Threading;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using Microsoft.Extensions.Hosting;

namespace UtilityBox.App.Server.Services
{
    public class HardwareMonitorHostService : IHostedService
    {
        private Computer ComputerMonitor { get; }

        public HardwareMonitorHostService()
        {
            ComputerMonitor = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true,
                IsPsuEnabled = true
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ComputerMonitor.Open();
            ComputerMonitor.Accept(new UpdateVisitor());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
    
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (var subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }

        public void VisitSensor(ISensor sensor)
        {
            // ignore
        }

        public void VisitParameter(IParameter parameter)
        {
            // ignore
        }
    }
}