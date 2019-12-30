using System;
using Hexio.AspNetCore.Logging.Enrichers;
using Hexio.AspNetCore.Logging.Enrichers.Request;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Hexio.AspNetCore.Logging
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder AddRequestLogging(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                //The StartupErrorLogger does not capture all startup errors, it might be dependent on the start order
                services.AddSingleton<IStartupFilter, StartupErrorLogger>();
                services.AddSingleton<IStartupFilter, LifetimeLogger>();
                services.AddSingleton<RequestMiddleware>();

                services.AddSingleton<IStartupFilter>(_ => new StartupFilter(builder =>
                {
                    builder.UseWhen(httpContext => httpContext.Request.Path != "/health", x =>
                    {
                        x.UseMiddleware<GlobalRequestMiddleware>();
                        x.UseMiddleware<RequestMiddleware>();
                    });
                }));

                //Global
                services.AddSingleton<GlobalRequestMiddleware>();
                services.AddSingleton<HttpContextEnricher<IGlobalRequestEnricher>>();

                //Request
                services.AddSingleton<HttpContextEnricher<IRequestEnricher>>();
                services.AddSingleton<IRequestEnricher, ControllerNameEnricher>();
                services.AddSingleton<IRequestEnricher, ControllerActionNameEnricher>();
                services.AddSingleton<IRequestEnricher, UserAgentEnricher>();
                services.AddSingleton<IRequestEnricher, RefererEnricher>();
                services.AddSingleton<IRequestEnricher, ContentTypeEnricher>();
                services.AddSingleton<IRequestEnricher, RequestBodyEnricher>();
                services.AddSingleton<IRequestEnricher, RouteValuesEnricher>();
                services.AddSingleton<IRequestEnricher, IpAddressEnricher>();
            });

            return hostBuilder;
        }
        
        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder builder)
        {
            builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var settings = hostingContext.Configuration.GetSection("ElasticSearch").Get<ElasticsearchSettings>();

                if (settings is null)
                {
                    throw new InvalidOperationException("Could not resolve Elasticsearch settings from config");
                }

                Serilog.Debugging.SelfLog.Enable(Console.Out);

                loggerConfiguration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                    .Enrich.With<ServiceNameEnricher>()
                    .Enrich.With<RestEaseExceptionEnricher>()
                    .Enrich.With<OutOfSocketsEnricher>()
//                    .Enrich.With(new EnvironmentEnricher(hostingContext.HostingEnvironment.EnvironmentName))
                    .Enrich.FromLogContext()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(settings.Url))
                    {
                        CustomFormatter = new Formatter(),
                        ModifyConnectionSettings = config =>
                        {
                            return config.BasicAuthentication(settings.Username, settings.Password);
                        },
                        MinimumLogEventLevel = LogEventLevel.Information,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    })
                    .WriteTo.Console(LogEventLevel.Information);
            });

            return builder;
        }
    }
}