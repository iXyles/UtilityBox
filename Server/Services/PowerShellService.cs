﻿using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBox.App.Server.Services
{
    public class PowerShellService
    {
        private readonly RunspacePool _rsPool;

        public PowerShellService()
        {
            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            _rsPool = RunspaceFactory.CreateRunspacePool(defaultSessionState);
            _rsPool.SetMinRunspaces(1);
            _rsPool.ThreadOptions = PSThreadOptions.UseNewThread;
       
            // open the pool.
            _rsPool.Open();
        }
        
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