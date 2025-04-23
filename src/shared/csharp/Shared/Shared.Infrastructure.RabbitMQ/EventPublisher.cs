using MassTransit;
using Shared.Infrastructure.RabbitMQ.Abstractions;

namespace Shared.Infrastructure.RabbitMQ;

public class EventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task ReplyAsync<TReply>(TReply message)
        where TReply : class
    {
        if (publishEndpoint is not ConsumeContext context)
            throw new Exception("Reply does not have context.");

        await context.RespondAsync(message);
    }

    public async Task Publish<T>(T message, CancellationToken cancellationToken)
        where T : class
    {
        if (publishEndpoint is ConsumeContext context && context.ResponseAddress != null)
        {
            await publishEndpoint.Publish(message, c =>
            {
                c.ResponseAddress = context.ResponseAddress;
                c.RequestId = context.RequestId;
            }, cancellationToken);
            return;
        }

        await publishEndpoint.Publish(message, cancellationToken);
    }
}