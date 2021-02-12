using System.Reflection;
using Autofac;

namespace Hexio.AspNetCore.ErrorHandling
{
    public static class AutofacExtensions
    {
        public static void ScanForErrorHandlers(this ContainerBuilder builder, Assembly assembly)
        {
            builder
                .RegisterAssemblyTypes(assembly)
                .AssignableTo<IErrorHandler>()
                .AsImplementedInterfaces();
        }
    }
}
