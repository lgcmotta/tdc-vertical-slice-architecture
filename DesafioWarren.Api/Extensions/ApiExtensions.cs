using DesafioWarren.Api.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DesafioWarren.Api.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddRoutingWithLowerCaseUrls(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRouting(configureOptions =>
            {
                configureOptions.LowercaseUrls = true;
                configureOptions.LowercaseQueryStrings = true;
            });

            return serviceCollection;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(corsOptions => corsOptions.AddDefaultPolicy(policyBuilder => policyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()));

            return serviceCollection;
        }

        public static IServiceCollection ConfigureApiVersion(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersion.Default;

                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            return serviceCollection;

        }

        public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "DesafioWarren.Api", Version = "v1" });
            });

            return serviceCollection;
        }
        
        public static IMvcBuilder ConfigureControllers(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddControllers(configure =>
                configure.Conventions.Add(new RouteTokenTransformerConvention(new UriOutboundParameterTransformer())));
        }

    }
}