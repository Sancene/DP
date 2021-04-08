using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NATS.Client;
using SharedLib;

namespace RankCalculator
{
    public class RankCalculator : IDisposable
    {
        private readonly IConnection _connection;
        private readonly ILogger _logger;
        private readonly IAsyncSubscription _subscription;

        public RankCalculator(ILogger logger, IStorage storage)
        {
            _logger = logger;
            _connection = NatsFactory.GetNatsConnection();
            _subscription = _connection.SubscribeAsync(Constants.RankKey, "rank", (_, args) =>
            {
                var id = Encoding.UTF8.GetString(args.Message.Data);
                var shard = storage.LoadShard(id);

                var text = storage.Load(shard, Constants.TextKeyPrefix + id);
                var rank = GetRank(text);
                _logger.LogInformation("Shard: " + shard + ", id: " + id + ", text: " + text + ", rank: " + rank);
                storage.Store(shard, Constants.RankKeyPrefix + id, rank.ToString(CultureInfo.InvariantCulture));

                var rankData = new RankMessage { Id = id, Rank = rank };
                _connection.Publish(Constants.RankKeyCalculated,
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(rankData)));
            });
        }

        public void Dispose()
        {
            _logger.LogInformation("RankCalculator is disposing...");
            _subscription.Dispose();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Subscribe()
        {
            _logger.LogInformation("RankCalculator subscriptions started");
            _subscription.Start();
        }

        private static double GetRank(string text)
        {
            if (text.Length == 0) return 0d;
            return 1d * text.Count(x => !char.IsLetter(x)) / text.Length;
        }
    }
}