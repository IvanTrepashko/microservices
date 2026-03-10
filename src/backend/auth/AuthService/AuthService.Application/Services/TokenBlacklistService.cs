using AuthService.Application.Services.Abstractions;

namespace AuthService.Application.Services;

public class TokenBlacklistService : ITokenBlacklistService
{
    public Task AddToBlacklistAsync(string jti, TimeSpan ttl, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInBlacklistAsync(string jti, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
