using AuthService.Infrastructure.Identity;

namespace AuthService.Application.Services.Abstractions;

public interface IJwtService
{
    string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles);

    string GenerateRefreshToken();
}
