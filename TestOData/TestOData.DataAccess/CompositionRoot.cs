using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TestOData.DataAccess.Repositories;
using TestOData.Interfaces.DataAccess;

[assembly: InternalsVisibleTo("TestOData.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TestOData.DataAccess
{
    public static class CompositionRoot
    {
        public static IServiceCollection RegisterDataAccess(this IServiceCollection services)
        {
            // Services
            services.AddTransient<IBooksRepository, BooksRepository>();

            return services;
        }
    }
}
