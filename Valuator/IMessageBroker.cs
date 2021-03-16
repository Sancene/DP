namespace Valuator
{
    public interface IMessageBroker
    {
        void Publish(string key, string value);
    }
}