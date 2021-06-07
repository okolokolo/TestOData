using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TestOData.Interfaces.Service;
using TestOData.Service.Services;

[assembly: InternalsVisibleTo("TestOData.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TestOData.Service
{
    public static class CompositionRoot
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Services
            services.AddTransient<IBooksService, BooksService>();

            return services;
        }
    }
}
