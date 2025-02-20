using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Tars.Adapter.Rest.DI;
using Tars.Contracts.Adapter.Rest.Response;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.Rest.DI
{
    public static class RestAdapterDI
    {
        public static ContainerBuilder ConfigureAuthorizationRestAdapter(this ContainerBuilder builder)
        {
            builder.AddResponseWrapperMiddleware<DefaultApiResponse<object>>();
            return builder;
        }

        public static IServiceCollection ConfigureAuthorizationRestAdapterControllers(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddApplicationPart(typeof(RestAdapterDI).Assembly)
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });

            services.AddRouting(options => options.LowercaseUrls = false);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pottmayer Ecommerce Authorization Service v1",
                    Version = "v1",
                    Description = "",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
