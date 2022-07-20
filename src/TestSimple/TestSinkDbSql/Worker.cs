using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestSinkDbSql
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Excute worker");
            _logger.LogInformation("Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}. CustomProperty1: {MachineName}",
                    DateTimeOffset.Now, "match");
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Worker stopping ...");
        }
    }
}