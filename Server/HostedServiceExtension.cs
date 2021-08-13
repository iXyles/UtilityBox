using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UtilityBox.App.Server
{
    public static class HostedServiceExtension
    {
        public static IServiceCollection AddHostedServiceAndSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(
            this IServiceCollection services
            )
            where TService : class, IHostedService
            => 
                services.AddSingleton<TService>()
                    .AddHostedService(p => p.GetService<TService>());
    }
}