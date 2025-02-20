using Autofac;
using Tars.Adapter.Cache.Memory.DI;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.Cache.DI
{
    public static class CacheAdapterDI
    {
        public static ContainerBuilder ConfigureAuthorizationCacheAdapter(this ContainerBuilder builder)
        {
            builder.ConfigureTarsMemoryCacheAdapter();
            return builder;
        }
    }
}
