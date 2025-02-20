using Autofac;
using Tars.Adapter.UserProvider.DI;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.UserProvider.DI
{
    public static class UserProviderAdapterDI
    {
        public static ContainerBuilder ConfigureAuthorizationUserProviderAdapter(this ContainerBuilder builder)
        {
            builder.ConfigureTarsUserProviderAdapter();
            return builder;
        }
    }
}
