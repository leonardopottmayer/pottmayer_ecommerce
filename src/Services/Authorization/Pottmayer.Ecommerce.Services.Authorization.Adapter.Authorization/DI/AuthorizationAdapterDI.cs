using Autofac;
using Tars.Adapter.Authorization.DI;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.Authorization.DI
{
    public static class AuthorizationAdapterDI
    {
        public static ContainerBuilder ConfigureAuthorizationAuthorizationAdapter(this ContainerBuilder builder)
        {
            builder.AddPasswordHasher()
                   .AddAuthService()
                   .AddAuthorizationMiddlewareResultHandler();

            return builder;
        }
    }
}
