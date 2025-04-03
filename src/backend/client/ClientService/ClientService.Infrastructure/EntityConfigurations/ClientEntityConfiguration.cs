using ClientService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientService.Infrastructure.EntityConfigurations;

public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(e => e.Id);

        builder.OwnsOne(
            c => c.Email,
            email =>
            {
                email.Property(e => e.Value).HasColumnName("Email").IsRequired().HasMaxLength(320);
            }
        );

        builder.OwnsOne(
            c => c.PhoneNumber,
            phoneNumber =>
            {
                phoneNumber.Property(e => e.Value)
                    .HasColumnName("PhoneNumber")
                    .IsRequired()
                    .HasMaxLength(15);
            }
        );

        builder.HasIndex(e => e.PhoneNumber).IsUnique();

        builder.HasOne(e => e.Address).WithOne().HasForeignKey<ClientAddress>(e => e.ClientId);
    }
}
