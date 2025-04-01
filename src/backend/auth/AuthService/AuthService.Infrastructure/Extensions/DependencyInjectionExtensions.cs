using AuthService.Application.Models.Identity;
using AuthService.Infrastructure.Configuration;
using AuthService.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsConfigure>();

        return services;
    }

    public static IServiceCollection ConfigurePostgres(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"))
        );

        services.AddDbContext<AuthContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"))
        );

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}