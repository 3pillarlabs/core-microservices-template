using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Services.Logger;
using Core.Services.Filters;
using Core.Services.Configurations;
using Core.Services.Repositories.Database;

namespace Core.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            ConfigurationBinder.Bind(Configuration.GetSection("AppSettings"), appSettings);
            services.AddSingleton<IAppSettings>(appSettings);
            services.AddSingleton<CustomAuthorize>();
            services.AddSingleton<IDatabaseRepository,DatabaseRepository>();
            services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.None)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("HyperLogFilter", LogLevel.Trace)
                    .AddConsole();
            });
            services.AddScoped<CustomExceptionFilter>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<LogResponseMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
