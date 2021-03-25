using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
namespace SharedLib
{
    public class RedisStorage : IStorage
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly string _hostName;

        public RedisStorage()
        {
            _hostName = Constants.HostName;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_hostName);
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
            return _connectionMultiplexer.GetServer(_hostName, Constants.Port).Keys().Select(x => x.ToString());
        }
        public bool DoesKeyExist(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return db.KeyExists(key);
        }
    }
}
