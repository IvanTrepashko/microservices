using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Infrastructure;

namespace Shared.Core.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddTransient<IClock, Clock>();

        return services;
    }
}