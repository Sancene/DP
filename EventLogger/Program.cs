using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventsLogger
{
    public static class Program
    {
        private static async Task Main()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            using var eventLogger = new EventsLogger(loggerFactory.CreateLogger<EventsLogger>());
            eventLogger.Subscribe();
            await Task.Delay(-1);
        }
    }
}