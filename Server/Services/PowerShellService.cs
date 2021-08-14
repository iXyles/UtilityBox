using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBox.App.Server.Services
{
    /**
     * Everything that has to do with powershell currently has a known memory leak and
     * the issue is coming from Microsofts SDK all together, which makes it impossible for me
     * to do any changes to resolve those memory leaks for now. (Awaiting fixes & .NET 6 to resolve those)
     */
    public class PowerShellService
    {
        private readonly RunspacePool _rsPool;

        public PowerShellService()
        {
            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            _rsPool = RunspaceFactory.CreateRunspacePool(defaultSessionState);
            _rsPool.SetMinRunspaces(0);
            _rsPool.CleanupInterval = TimeSpan.FromSeconds(5);
            _rsPool.ThreadOptions = PSThreadOptions.UseNewThread;
       
            // open the pool.
            _rsPool.Open();
        }

        public Task<string> RunScriptAsync(string script)
            => RunScriptAsync(new List<string>{ script });
        
        public async Task<string> RunScriptAsync(List<string> scripts)
        {
            using var ps = PowerShell.Create();
            ps.RunspacePool = _rsPool;

            // specify the script code to run.
            scripts.ForEach(s => ps.AddScript(s));

            // execute the script and await the result.
            var results = await ps.InvokeAsync().ConfigureAwait(false);

            // convert all the ps objects results to string
            var builder = new StringBuilder();
            foreach (var obj in results)
                builder.AppendLine(obj.ToString());

            // return the result ot the requester
            return builder.ToString();
        }
    }
}