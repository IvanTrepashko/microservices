using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Core.Exceptions;

namespace AuthService.Application.Commands.Authentication;

public record LogoutCommand(ClaimsPrincipal Claims) : IRequest;

public class LogoutCommandHandler(
    ITokenBlacklistService tokenBlacklistService,
    UserManager<ApplicationUser> userManager,
    TimeProvider timeProvider
) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var jti = request
            .Claims.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)
            ?.Value;
        var userEmail = request.Claims.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(userEmail))
        {
            throw new UnauthorizedException("invalid token");
        }

        var exp = long.Parse(request.Claims.FindFirstValue(JwtRegisteredClaimNames.Exp));
        var expTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
        var date = timeProvider.GetUtcNow().DateTime;
        var ttl = expTime - date;

        var user =
            await userManager.FindByEmailAsync(userEmail)
            ?? throw new NotFoundException<ApplicationUser>(userEmail);

        await tokenBlacklistService.AddToBlacklistAsync(jti, ttl, cancellationToken);

        user.RefreshToken = null;

        await userManager.UpdateAsync(user);
    }
}
