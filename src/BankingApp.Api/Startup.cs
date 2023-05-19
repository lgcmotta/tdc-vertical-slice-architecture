using Autofac;
using BankingApp.Api.Extensions;
using BankingApp.Application.Autofac;
using BankingApp.Application.AutoMapper;
using BankingApp.Application.Hubs;
using BankingApp.Application.Quartz;
using BankingApp.Application.Serilog;
using BankingApp.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Quartz;

namespace BankingApp.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    private const string ApplicationAssembly = "BankingApp.Application";

    private const string DomainAssembly = "BankingApp.Domain";

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

                options.InstanceName = nameof(BankingApp);
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
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "BankingApp.Api v1"));
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