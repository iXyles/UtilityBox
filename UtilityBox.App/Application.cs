using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UtilityBox.App
{
    public static class Application
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseElectron(args);
                    webBuilder.UseStartup<StartupApp>();
                });
    }
}