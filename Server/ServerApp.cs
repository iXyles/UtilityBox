using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UtilityBox.App.Server
{
    public class ServerApp
    {
        public async Task StartWebServer(Action<string> action)
        {
            try
            {
                await CreateHostBuilder(action).Build().RunAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{ex.Message}\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }
        }

        private IHostBuilder CreateHostBuilder(Action<string> action) =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(_ => new StartupApp(action));
                    webBuilder.UseUrls("http://localhost:47220", "http://*:47220");
                });
    }
}