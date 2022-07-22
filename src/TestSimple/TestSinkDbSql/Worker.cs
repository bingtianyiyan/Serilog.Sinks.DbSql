using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            // _logger.LogInformation("Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}.{test} CustomProperty1: {MachineName}",
                    DateTimeOffset.Now, "123", "match");
                await Task.Delay(1000, stoppingToken);
            }

            //  _logger.LogInformation("Worker stopping ...");
        }
    }
}