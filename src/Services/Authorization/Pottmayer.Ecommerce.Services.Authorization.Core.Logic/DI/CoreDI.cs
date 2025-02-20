using Autofac;
using Tars.Core.DI;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Logic.DI
{
    public static class CoreDI
    {
        public static ContainerBuilder ConfigureAuthorizationCore(this ContainerBuilder builder)
        {
            builder.AddDateProvider()
                   .RegisterCommandHandlers(typeof(CoreDI).Assembly);

            return builder;
        }
    }
}
