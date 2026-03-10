using System.Security.Claims;
using AuthService.Application.Options;
using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Core.Exceptions;

namespace AuthService.Application.Commands.Authentication;

public record RefreshTokenCommand(string AccessToken, string RefreshToken)
    : IRequest<RefreshTokenCommandResponse>;

public record RefreshTokenCommandResponse(string AccessToken, string RefreshToken);

public class RefreshTokenCommandHandler(
    IJwtService jwtService,
    UserManager<ApplicationUser> userManager,
    TimeProvider timeProvider,
    IOptions<JwtOptions> jwtOptions
) : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
    public async Task<RefreshTokenCommandResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var principal = jwtService.ValidateExpiredAccessToken(request.AccessToken);

        var userEmail = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new Exception();
        }

        var user =
            await userManager.FindByEmailAsync(userEmail)
            ?? throw new NotFoundException<ApplicationUser>(userEmail);

        if (!string.Equals(user.RefreshToken, request.RefreshToken, StringComparison.Ordinal))
        {
            throw new Exception("Invalid refresh token"); // create business exception
        }

        var date = timeProvider.GetUtcNow().UtcDateTime;
        if (user.RefreshTokenExpiryTime < date)
        {
            throw new Exception("Refresh token expired");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

        var newAccessToken = jwtService.GenerateAccessToken(user, role);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = date.AddDays(jwtOptions.Value.RefreshTokenExpirationDays);

        await userManager.UpdateAsync(user);

        return new RefreshTokenCommandResponse(newAccessToken, newRefreshToken);
    }
}
