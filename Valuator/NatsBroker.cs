using System.Text;
using NATS.Client;

namespace Valuator
{
    public class NatsBroker : IMessageBroker
    {
        public void Publish(string key, string value)
        {
            using var connection = new ConnectionFactory().CreateConnection();
            connection.Publish(key, Encoding.UTF8.GetBytes(value));
        }
    }
}