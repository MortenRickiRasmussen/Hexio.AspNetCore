using Autofac;
using Hexio.AspNetCore.Logging.Enrichers.Request;

namespace Hexio.AspNetCore.Logging
{
    public static class AutofacExtensions
    {
        public static void AddRequestEnricher<T>(this ContainerBuilder builder) where T : IRequestEnricher
        {
            builder
                .RegisterType<T>()
                .As<IRequestEnricher>()
                .SingleInstance();
        }

        public static void AddGlobalRequestEnricher<T>(this ContainerBuilder builder) where T : IGlobalRequestEnricher
        {
            builder
                .RegisterType<T>()
                .As<IGlobalRequestEnricher>()
                .SingleInstance();
        }
    }
}