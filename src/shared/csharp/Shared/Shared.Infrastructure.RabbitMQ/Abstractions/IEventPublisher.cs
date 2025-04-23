namespace Shared.Infrastructure.RabbitMQ.Abstractions;

public interface IEventPublisher
{
    Task Publish<T>(T message, CancellationToken cancellationToken)
    where T : class;

    Task ReplyAsync<TReply>(TReply message)
        where TReply : class;
}