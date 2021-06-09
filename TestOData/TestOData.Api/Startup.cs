
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using RSM.Core.Logging.Extensions.Adapters;
using System.Linq;
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
            services.AddDbContext<BookStoreContext>(opt => opt.UseInMemoryDatabase("BookLists"));

            services.AddRouting();

            services
                .RegisterServices()
                .RegisterDataAccess()
                .AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
                .Configure<IISServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                })
                //.AddTransient<IResponseFormatter, DefaultResponseFormatter>()
                .AddControllers()
                .AddNewtonsoftJson();
            //.AddOData(opt => {
            //    //opt
            //    opt.AddModel("odata", GetEdmModel());
            //});

            services.AddOData();
            // Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerService();
            services.AddSwaggerGen();
            services.AddFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO: this was not working for some reason
            //app.UseSwaggerApp()
            //.ConfigureResponseWrapper(new WrapperMiddlewareOptions
            //{
            //    WhiteListPaths = new List<string> { "/api/v1" }
            //});


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
