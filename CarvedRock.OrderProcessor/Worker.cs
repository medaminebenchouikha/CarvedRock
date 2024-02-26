using Serilog;

namespace CarvedRock.OrderProcessor
{
    public class Worker : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Log.IsEnabled((Serilog.Events.LogEventLevel)LogLevel.Information))
                {
                    Log.Information("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
