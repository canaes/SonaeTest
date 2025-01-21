using Microsoft.Extensions.Options;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Services;
using SonaeTestSol.Services.Base;
using SonaeTestSol.Services.HostedService;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SonaeTestSol.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerConfiguration>();
            services.AddSingleton<IErrorService, ErrorService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IStockService, StockService>();


            //services.AddScoped<IErrorService, ErrorService>();

            services.AddHostedService<StockHostService>();
            return services;
        }
    }
}
