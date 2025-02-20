using Autofac;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Authorization.DI;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Cache.DI;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Data;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Data.DI;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Rest.DI;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.UserProvider.DI;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.MapperProfiles;
using Pottmayer.Ecommerce.Services.Authorization.Core.Logic.DI;
using System.Reflection;
using Tars.Adapter.Authorization.Extensions;
using Tars.Adapter.Rest.Middlewares;
using Tars.Contracts.Adapter.Rest.Response;
using Tars.Core.DI;

namespace Pottmayer.Ecommerce.Services.Authorization.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthProfile).GetTypeInfo().Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddHttpContextAccessor();
            services.AddCors();

            services.ConfigureAuthorizationRestAdapterControllers();

            services.AddEndpointsApiExplorer();

            services.AddAuthorization();
            services.ConfigureAuthentication(Configuration);

            services.ConfigureDbContext<AppDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pottmayer MTV v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.ConfigureDefaultAuthMiddlewares();

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware<ResponseWrappingMiddleware<DefaultApiResponse<object>>>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(name: "apiVersion", pattern: "api/v{version}/{controller=Home}/{action=Index}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddApplicationService();

            builder.ConfigureAuthorizationCore();
            builder.ConfigureAuthorizationDataAdapter();
            builder.ConfigureAuthorizationAuthorizationAdapter();
            builder.ConfigureAuthorizationRestAdapter();
            builder.ConfigureAuthorizationUserProviderAdapter();
            builder.ConfigureAuthorizationCacheAdapter();
        }
    }
}
