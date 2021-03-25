using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NATS.Client;
using SharedLib;

namespace EventsLogger
{
    public class EventsLogger : IDisposable
    {
        private readonly IConnection _connection;
        private readonly ILogger<EventsLogger> _logger;
        private readonly IAsyncSubscription _subscription;

        public EventsLogger(ILogger<EventsLogger> logger)
        {
            _logger = logger;
            _connection = NatsFactory.GetNatsConnection();

            _subscription = _connection.SubscribeAsync("rank_calculator.rank_calculated", (sender, args) =>
            {
                var rank = JsonSerializer.Deserialize<RankMessage>(args.Message.Data);
                logger.LogInformation($"Event: {args.Message.Subject}\n{rank}; Text with id {rank.Id} has rank {rank.Rank}");
            });

            _subscription = _connection.SubscribeAsync("valuator.similarity_calculated", (sender, args) =>
            {
                var similarity = JsonSerializer.Deserialize<SimilarityMessage>(args.Message.Data);
                logger.LogInformation($"Event: {args.Message.Subject}\n{similarity}; Text with id {similarity.Id} has similarity {similarity.Similarity}");
            });
        }

        public void Dispose()
        {
            _logger.LogInformation("EventLogger is disposing...");
            _subscription.Dispose();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Subscribe()
        {
            _logger.LogInformation("EventLogger subscriptions started");
            _subscription.Start();
        }
    }
}