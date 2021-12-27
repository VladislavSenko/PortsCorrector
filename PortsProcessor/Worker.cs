using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PortsProcessor.Services;

namespace PortsProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly PortsProcessorService _portsProcessorService;
        public Worker(ILogger<Worker> logger, PortsProcessorService portsProcessorService)
        {
            _logger = logger;
            _portsProcessorService = portsProcessorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                await _portsProcessorService.StartProcessing();
            }
        }
    }
}