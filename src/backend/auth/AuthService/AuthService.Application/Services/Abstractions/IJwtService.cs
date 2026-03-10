using System.Security.Claims;
using AuthService.Infrastructure.Identity;

namespace AuthService.Application.Services.Abstractions;

public interface IJwtService
{
    string GenerateAccessToken(ApplicationUser user, string role);

    string GenerateRefreshToken();

    ClaimsPrincipal ValidateExpiredAccessToken(string accessToken);
}
