namespace WebApi.MessageBroker
{
    public interface IPublishMessage
    {
        Task SendMessage<T>(T message, CancellationToken cancellationToken = default);
    }
}
