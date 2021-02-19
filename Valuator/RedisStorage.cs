using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using StackExchange.Redis;
using System;

namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly string host = "localhost";
        private readonly int port = 6379;

        public RedisStorage(ILogger<RedisStorage> logger)
        {
            _logger = logger;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(host);
        }

        public void Store(string key, string value)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            if (!db.StringSet(key, value))
            {
                _logger.LogWarning("Failed to save", key, ": ",value);
            }
        }

        public string Load(string key)
        {
            IDatabase db = _connectionMultiplexer.GetDatabase();
            if (db.KeyExists(key))
            {
                return db.StringGet(key);
            }
            _logger.LogWarning("Key ", key, " doesn't exist");
            return string.Empty;
        }

        public List<string> GetKeys()
        {
            List<string> data = new List<string>();

            var keys = _connectionMultiplexer.GetServer(host, port).Keys();

            foreach (var item in keys)
            {
                data.Add(item.ToString());
                Console.WriteLine(item.ToString());
            }

            return data;
        }
    }
}
