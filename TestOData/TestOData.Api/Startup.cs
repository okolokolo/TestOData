
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RSM.Core.AspNet.Extensions;
using RSM.Core.AspNet.Response;
using RSM.Core.Logging.Extensions.Adapters;
using System.Collections.Generic;
using TestOData.DataAccess;
using TestOData.Model;
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
            services.AddRouting();

            services
                .RegisterServices()
                .RegisterDataAccess()
                .AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
                .Configure<IISServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                })
                .AddTransient<IResponseFormatter, DefaultResponseFormatter>()
                .AddControllers()
                .AddOData(opt => opt.AddModel("odata", GetEdmModel())); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureSwagger("TemplateWebApp")
            .ConfigureResponseWrapper(new WrapperMiddlewareOptions
            {
                WhiteListPaths = new List<string> { "/api/v1" }
            })
            .UseHttpsRedirection()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Book>("Books");
            builder.EntitySet<Press>("Presses");
            return builder.GetEdmModel();
        }

    }
}
