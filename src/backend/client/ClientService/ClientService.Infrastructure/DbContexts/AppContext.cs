using ClientService.Domain.Entities;
using ClientService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Infrastructure.DbContexts;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientPreferences> ClientPreferences { get; set; }
    public DbSet<ClientAddress> ClientAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(DependencyInjectionExtensions).Assembly
        );
    }
}
