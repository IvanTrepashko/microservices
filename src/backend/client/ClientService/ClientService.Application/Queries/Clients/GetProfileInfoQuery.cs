using MediatR;

namespace ClientService.Application.Queries.Clients;

public record GetProfileInfoQuery(long ClientId) : IRequest<GetProfileInfoResponse>;

public record GetProfileInfoResponse();

public class GetProfileInfoQueryHandler : IRequestHandler<GetProfileInfoQuery, GetProfileInfoResponse>
{
    public Task<GetProfileInfoResponse> Handle(GetProfileInfoQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}