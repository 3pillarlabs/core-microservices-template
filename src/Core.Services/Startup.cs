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
            var loggerFactory = new LoggerFactory();
            loggerFactory
               .WithFilter(new FilterLoggerSettings
               {
                    { "Microsoft", Microsoft.Extensions.Logging.LogLevel.None },
                    { "System", Microsoft.Extensions.Logging.LogLevel.Warning },
                    { "HyperLogFilter", Microsoft.Extensions.Logging.LogLevel.Trace }
               })
               .AddConsole();
            loggerFactory.AddDebug();
            services.AddMvc(_ =>
            {
                _.Filters.Add(new CustomExceptionFilter(appSettings, loggerFactory));
            });
            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
               .WithFilter(new FilterLoggerSettings
               {
                    { "Microsoft",LogLevel.None },
                    { "System", LogLevel.Warning },
                    { "HyperLogFilter", LogLevel.Trace }
               })
               .AddConsole();
            loggerFactory.AddDebug();          
            app.UseMiddleware<LogResponseMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
