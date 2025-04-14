using AuthService.Application.Options;
using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

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
            ?? throw new Exception("User not found"); //todo: create errors

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            false
        );

        if (!signInResult.Succeeded)
        {
            // todo: unauthorized
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