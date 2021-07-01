using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using TestOData.DataAccess;
using TestOData.Service;

namespace TestOData.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            
            services.AddDbContext<BookStoreContext>(opt => opt.UseInMemoryDatabase("BookLists"));

            services.AddRouting();

            services
                .RegisterServices()
                .RegisterDataAccess()
                .Configure<IISServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                })
                .AddControllers()
                .AddNewtonsoftJson();

            // Register OData
            services.AddOData();

            // Register the Swagger generator use the class to define the odata parameters in swagger
            services.AddSwaggerGen(c => c.OperationFilter<ODataParametersSwaggerDefinition>());

            // Adds Formatters so you can build odata endpoints in Swagger
            services.AddFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test ODATA API V1");
            });

            app.UseHttpsRedirection()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().Expand().OrderBy().MaxTop(100).Count();
                endpoints.MapControllers();
            });

        }

    }
}
