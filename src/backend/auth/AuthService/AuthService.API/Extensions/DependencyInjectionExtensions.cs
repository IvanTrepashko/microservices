using AuthService.API.Configuration;

namespace AuthService.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsConfigure>();

        return services;
    }
}