using System;

namespace AuthService.Application.Services.Abstractions;

public interface ITokenBlacklistService
{
    Task<bool> IsInBlacklistAsync(string jti, CancellationToken cancellationToken);

    Task AddToBlacklistAsync(string jti, TimeSpan ttl, CancellationToken cancellationToken);
}
