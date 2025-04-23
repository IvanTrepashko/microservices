using MassTransit;

namespace Shared.Infrastructure.RabbitMQ.Definitions;

public class ConsumerQuorumDefinition<TConsumer> : ConsumerDefinition<TConsumer> where TConsumer : class, IConsumer
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            rmq.SetQuorumQueue();
    }
}