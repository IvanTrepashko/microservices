using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthService.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var userContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            var authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await userContext.Database.MigrateAsync();
            await authContext.Database.MigrateAsync();
            await SeedRoles(roleManager);
        }

        return host;
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}