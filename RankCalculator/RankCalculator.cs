using System;
using System.Globalization;
using System.Linq;
using System.Text;
using NATS.Client;
using Valuator;

namespace RankCalculator
{
    public class RankCalculator
    {
        private readonly IConnection _connection;
        private readonly IAsyncSubscription _subscription;

        public RankCalculator(IStorage storage)
        {
            _connection = new ConnectionFactory().CreateConnection();

            _subscription = _connection.SubscribeAsync(Constants.RankKeyPrefix, (_, args) =>
            {
                var id = Encoding.UTF8.GetString(args.Message.Data);
                var textKey = Constants.TextKeyPrefix + id;
                if (!storage.DoesKeyExist(textKey)) return;
                var text = storage.Load(textKey);
                var rank = GetRank(text);
                storage.Store(Constants.RankKeyPrefix + id, rank.ToString(CultureInfo.InvariantCulture));
            });
        }

        public void Run()
        {
            _subscription.Start();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

            _connection.Drain();
            _connection.Close();
        }

        private static double GetRank(string text)
        {
            if (text.Length == 0) return 0d;
            return 1d * text.Count(x => !char.IsLetter(x)) / text.Length;
        }
    }
}