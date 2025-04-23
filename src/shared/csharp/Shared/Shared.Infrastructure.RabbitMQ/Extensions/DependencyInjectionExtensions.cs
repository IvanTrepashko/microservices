using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.RabbitMQ.Abstractions;
using Shared.Infrastructure.RabbitMQ.Configuration;
using Shared.Infrastructure.RabbitMQ.Options;

namespace Shared.Infrastructure.RabbitMQ.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        ConfigureBusAction configureBusAction,
        ConfigureRabbitMqAction configureRabbitMqAction)
    {
        services.TryAddSingleton<IEndpointNameFormatter, ApplicationEndpointNameFormatter>();
        services.AddScoped<IEventPublisher, EventPublisher>();

        services.AddOptions<RabbitMqOptions>()
            .Validate(c => !string.IsNullOrEmpty(c.ConnectionString), "Connection string is empty.");

        services.ConfigureOptions<RabbitMqOptionsConfigure>();

        services.AddMassTransit(c =>
        {
            configureBusAction?.Invoke(c);

            c.UsingRabbitMq((context, config) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptionsMonitor<RabbitMqOptions>>().CurrentValue;
                var uri = new Uri(rabbitMqOptions.ConnectionString);

                config.Host(uri, cfg =>
                {
                    cfg.PublisherConfirmation = true;
                });

                config.AutoStart = true;

                config.ConfigureEndpoints(context);
                configureRabbitMqAction?.Invoke(context, config);

                config.SendTopology.ConfigureErrorSettings = settings =>
                {
                    settings.Lazy = true;
                    settings.SetQueueArgument("x-queue-type", "classic");
                };

                config.SendTopology.ConfigureDeadLetterSettings = settings =>
                {
                    settings.Lazy = true;
                    settings.SetQueueArgument("x-queue-type", "classic");
                };
            });
        });

        return services;
    }
}