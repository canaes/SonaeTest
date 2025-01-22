using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SonaeTestSol.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Services.HostedService
{
    public class StockHostService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private Timer _timer;
        private readonly IHostEnvironment _hostEnvironment;
        private static bool IsLoading = false;

        public StockHostService(IServiceProvider services, IHostEnvironment hostEnvironment)
        {
            _services = services;
            _hostEnvironment = hostEnvironment;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var serv = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    await serv.ProcessExpireOrder();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
