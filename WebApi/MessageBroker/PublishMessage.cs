using MassTransit;
using WebApi.Services;

namespace WebApi.MessageBroker;
public class PublishMessage : IPublishMessage
{
    private readonly IPublishEndpoint _publish;
    public PublishMessage(IPublishEndpoint publish)
    {
        _publish = publish;
    }
    public async Task SendMessage<T>(T message, CancellationToken cancellationToken = default)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message), "Message cannot be null");
        }

        await _publish.Publish(message, cancellationToken);
    }
}