using System;
using System.IO;
using System.Linq;
using DotNetify;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server
{
    public class StartupApp
    {
        private readonly Action<string> _webServerAddressInvoker;
        readonly string UtilityBoxSpecificOriginsPolicy = "_utilityBoxOriginPolicy";

        public StartupApp(Action<string> webServerAddressInvoker)
        {
            _webServerAddressInvoker = webServerAddressInvoker;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy(name: UtilityBoxSpecificOriginsPolicy,
                    builder =>
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()));
                        
            // Hosted services
            services.AddHostedServiceAndSingleton<HardwareMonitorHostService>();
            
            // Services
            services.AddSingleton<PowerShellService>();
            services.AddSingleton<WindowsAppService>();
            services.AddSingleton<UpdateRegistryService>();
            
            services.AddMemoryCache();
            services.AddSignalR();
            services.AddDotNetify();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseCors(UtilityBoxSpecificOriginsPolicy);
            
            app.UseWebSockets();
            app.UseDotNetify();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "wwwroot")),
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DotNetifyHub>("/dotnetify");
                endpoints.MapControllers();
            });

            app.Run(async context =>
            {
                using var reader = new StreamReader(File.OpenRead($"{AppContext.BaseDirectory}/wwwroot/index.html"));
                await context.Response.WriteAsync(await reader.ReadToEndAsync());
            });
            lifetime.ApplicationStarted.Register(
                () => AppStartedInvoke(app.ServerFeatures.Get<IServerAddressesFeature>()));
        }

        private void AppStartedInvoke(IServerAddressesFeature addressesFeature)
        {
            var address = addressesFeature.Addresses.Any(x => x.StartsWith("http:"))
                ? addressesFeature.Addresses.First(x => x.StartsWith("http:"))
                : addressesFeature.Addresses.First();
            _webServerAddressInvoker?.Invoke(address);
        }
    }
}