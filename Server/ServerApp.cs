using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UtilityBox.App.Server
{
    public class ServerApp
    {
        public void StartWebServer(Action<string> action) => CreateHostBuilder(action).Build().RunAsync();

        private IHostBuilder CreateHostBuilder(Action<string> action) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(_ => new StartupApp(action));
                });
    }
}