using AuthService.Application.Errors;
using AuthService.Application.Options;
using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Core.Exceptions;

namespace AuthService.Application.Commands.Authentication;

public record LoginCommand(string Email, string Password) : IRequest<LoginCommandResponse>;

public record LoginCommandResponse(string AccessToken, string RefreshToken);

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtService jwtService,
    IOptionsMonitor<JwtOptions> jwtOptions
) : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException<ApplicationUser>(request.Email);

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            false
        );

        if (!signInResult.Succeeded)
        {
            throw new UnauthorizedException(
                AuthenticationErrors.InvalidCredentials.Code,
                AuthenticationErrors.InvalidCredentials.Message
            );
        }

        var roles = await userManager.GetRolesAsync(user);

        var accessToken = jwtService.GenerateAccessToken(user, roles);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
            jwtOptions.CurrentValue.RefreshTokenExpirationDays
        );

        await userManager.UpdateAsync(user);

        return new LoginCommandResponse(accessToken, refreshToken);
    }
}