using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using System;

namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisStorage()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(Constants.Host);
        }

        public void Store(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }

        public string Load(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }

        public IEnumerable<string> GetKeys()
        {
            var keys = _connectionMultiplexer.GetServer(Constants.Host, Constants.Port).Keys();
            return _connectionMultiplexer.GetServer(Constants.Host, Constants.Port).Keys().Select(x => x.ToString()).ToList();
        }
        public bool DoesKeyExist(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return db.KeyExists(key);
        }
    }
}
