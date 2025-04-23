using MassTransit;

namespace Shared.Infrastructure.RabbitMQ.Configuration;

public delegate void ConfigureRabbitMqAction(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator);

public delegate void ConfigureBusAction(IBusRegistrationConfigurator busConfigurator);