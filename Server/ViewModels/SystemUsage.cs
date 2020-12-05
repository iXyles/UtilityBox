using DotNetify;
using System.Diagnostics;
using System.Management;
using System.Timers;

namespace UtilityBox.App.Server.ViewModels
{
    public class SystemUsage : MulticastVM
    {
        private static Timer _timer;
        private static PerformanceCounter _cpuCounter;
        private static ManagementObjectSearcher _searcher;

        public float CurrentCpuUsage { get; private set; }
        public float CurrentMemoryUsage { get; private set; }

        public SystemUsage()
        {
            _searcher ??= new ManagementObjectSearcher(new ObjectQuery("SELECT * FROM Win32_OperatingSystem"));
            _cpuCounter ??= new PerformanceCounter("Processor", "% Processor Time", "_Total");

            if (_timer == null) 
            {
                _timer = new Timer(1000);
                _timer.Elapsed += (_, _) =>
                {
                    CurrentCpuUsage = _cpuCounter.NextValue();
                    
                    using var memoryLookup = _searcher.Get();
                    var totalMemory = 0L;
                    var totalUsage = 0L;
                    foreach (var result in memoryLookup)
                    {
                        if (long.TryParse(result["TotalVisibleMemorySize"].ToString(), out var total) &&
                            long.TryParse(result["FreePhysicalMemory"].ToString(), out var used))
                        {
                            totalMemory += total;
                            totalUsage += used;
                        }
                    }
                    CurrentMemoryUsage = ((float) totalUsage / totalMemory) * 100;

                    Changed(nameof(CurrentCpuUsage));
                    Changed(nameof(CurrentMemoryUsage));
                    PushUpdates();
                };
            }
           
            _timer.Start();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            _timer.Dispose();
            _timer = null;
        }
    }
}
