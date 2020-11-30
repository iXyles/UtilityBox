using DotNetify;
using System.Diagnostics;
using System.Threading;

namespace UtilityBox.App.Server.ViewModels
{
    public class SystemUsage : BaseVM
    {
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private readonly Timer _timer;

        public float CurrentCpuUsage {get; private set;}
        public float CurrentMemoryUsage { get; private set; }

        public SystemUsage()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            _timer = new Timer(state =>
            {
                CurrentCpuUsage = _cpuCounter.NextValue();
                CurrentMemoryUsage = _ramCounter.NextValue();

                Changed(nameof(CurrentCpuUsage));
                Changed(nameof(CurrentMemoryUsage));
                PushUpdates();
            }, null, 0, 1000);
        }

        public override void Dispose() => _timer.Dispose();
    }
}
