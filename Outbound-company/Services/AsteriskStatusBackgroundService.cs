using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Outbound_company.Services
{
    public class AsteriskStatusBackgroundService : BackgroundService
    {
        private readonly IAsteriskStatusService _asteriskStatusService;
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);

        public AsteriskStatusBackgroundService(IAsteriskStatusService asteriskStatusService)
        {
            _asteriskStatusService = asteriskStatusService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _asteriskStatusService.UpdateStatusAsync();
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
