using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Services.Logger;
using Core.Services.Filters;
using Core.Services.Configurations;
using Core.Services.Repositories.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

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
           // var connection = @"Server=NDI-LAP-587;Database=CoreServices;Trusted_Connection=True;";
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appSettings.ConnectionString));
            services.AddScoped<AppDBSeedData>();
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
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("doc", new Info() { Title = "Core Microservice" });
                opt.AddSecurityDefinition("apikey", new ApiKeyScheme { In = "header", Description = "Please pass apikey", Name = "apikey", Type = "apiKey" });
                opt.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "apikey", Enumerable.Empty<string>() }
                });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,AppDBSeedData seeder)
        {
            app.UseMiddleware<LogResponseMiddleware>();
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/doc/swagger.json", "DataService API");
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                if (!context.AllMigrationsApplied())
                {
                    context.Database.Migrate();
                    seeder.EnsureSeeded().Wait();
                }
            }
           
           
        }
    }
}
