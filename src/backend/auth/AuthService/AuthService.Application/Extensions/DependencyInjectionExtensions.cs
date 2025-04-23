using AuthService.Application.Services;
using AuthService.Application.Services.Abstractions;
using AuthService.Application.Validation.Behaviors;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.RabbitMQ.Extensions;

namespace AuthService.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtensions).Assembly);

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddRabbitMQ(cfg =>
        {
        },
        (context, cfg) =>
        {
            cfg.UseMessageScope(context);
        });

        return services;
    }
}