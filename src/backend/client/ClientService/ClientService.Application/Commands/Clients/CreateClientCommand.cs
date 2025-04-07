using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Application.Commands.Clients;

public record CreateClientProfileCommand(
    long ClientId,
    Name Name,
    Email Email,
    PhoneNumber PhoneNumber,
    ClientAddress Address,
    DateTime BirthDate,
    ClientGender Gender
) : IRequest;

public class CreateClientProfileCommandHandler(AppDbContext context)
    : IRequestHandler<CreateClientProfileCommand>
{
    public async Task Handle(
        CreateClientProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingClient =
            await context.Clients.FirstOrDefaultAsync(
                c => c.Id == request.ClientId,
                cancellationToken
            ) ?? throw new Exception("Client not found");

        var client = new Client(
            request.ClientId,
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.Address,
            request.BirthDate,
            ClientType.Individual,
            ClientStatus.Active,
            request.Gender,
            DateTime.UtcNow
        );

        await context.Clients.AddAsync(client, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
