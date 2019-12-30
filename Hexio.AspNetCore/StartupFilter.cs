using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Hexio.AspNetCore
{
    internal class StartupFilter : IStartupFilter
    {
        private readonly Action<IApplicationBuilder> _builder;

        public StartupFilter(Action<IApplicationBuilder> builder)
        {
            _builder = builder;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                _builder(builder);

                next(builder);
            };
        }
    }
}