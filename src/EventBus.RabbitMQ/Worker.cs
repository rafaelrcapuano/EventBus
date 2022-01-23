using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventBus.RabbitMQ
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly ILogger<Worker> _logger;

        public Worker(IBus bus, ILogger<Worker> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Publicando: {DateTimeOffset.Now}");

                await _bus.Publish<IMessage>(new
                {
                    Text = $"Hora atual: {DateTimeOffset.Now}"
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
