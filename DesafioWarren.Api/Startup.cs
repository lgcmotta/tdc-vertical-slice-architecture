using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Autofac;
using DesafioWarren.Api.Extensions;
using DesafioWarren.Application.Autofac;
using DesafioWarren.Application.AutoMapper;
using DesafioWarren.Application.Hubs;
using DesafioWarren.Application.Quartz;
using DesafioWarren.Application.Serilog;
using DesafioWarren.Infrastructure.EntityFramework;
using Microsoft.Identity.Web;
using Quartz;

namespace DesafioWarren.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private const string ApplicationAssembly = "DesafioWarren.Application";

        private const string DomainAssembly = "DesafioWarren.Domain";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureContainer(ContainerBuilder container)
        {
            container.AddAutofacModules();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            services.AddSignalR();
            
            services
                .AddHttpContextAccessor()
                .ConfigureSerilog(Configuration)
                .AddAutoMapperFromAssemblies(ApplicationAssembly)
                .AddAccountsDbContext(Configuration)
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = Configuration.GetConnectionString("Redis");

                    options.InstanceName = nameof(DesafioWarren);
                })
                .AddQuartzJobs()
                .AddQuartzHostedService(quartz => quartz.WaitForJobsToComplete = true)
                .ConfigureCors()
                .ConfigureApiVersion()
                .AddRoutingWithLowerCaseUrls()
                .ConfigureSwaggerGen()
                .AddControllers();

            services.ConfigureControllers();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json"
                    , "DesafioWarren.Api v1"));
            }

            app.UseCors()
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<AccountsHub>("/accounts/hub");
                });
        }
    }
}
