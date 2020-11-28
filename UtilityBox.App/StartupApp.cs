using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Hosting;
using VueCliMiddleware;

namespace UtilityBox.App
{
    public class StartupApp
    {
        private readonly IConfiguration _configuration;

        public StartupApp(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(opt =>
            {
                opt.RootPath = "ClientApp/dist";
            });
            services.AddControllers();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSpaStaticFiles();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseSpa(spa =>
            {
                // COMMENT FOR PRODUCTION
                #if DEBUG
                spa.UseProxyToSpaDevelopmentServer("https://localhost:8080");
                #endif
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (HybridSupport.IsElectronActive)
                ElectronBootstrap();
        }

        private async void ElectronBootstrap()
        {
            var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Show = false
            });

            await window.WebContents.Session.ClearCacheAsync();
            window.OnReadyToShow += () => window.Show();
            window.SetTitle("UtilityBox App");
        }
    }
}