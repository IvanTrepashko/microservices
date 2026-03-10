using AuthService.Application.Errors;
using AuthService.Application.Options;
using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Core.Exceptions;

namespace AuthService.Application.Commands.Authentication;

public record RegisterCommand(string Email, string Password, string UserName)
    : IRequest<RegisterCommandResponse>;

public record RegisterCommandResponse(string AccessToken, string RefreshToken);

public class RegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtService jwtService,
    IOptions<JwtOptions> options,
    TimeProvider timeProvider
) : IRequestHandler<RegisterCommand, RegisterCommandResponse>
{
    public async Task<RegisterCommandResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new BadRequestException(
                AuthenticationErrors.UserAlreadyExists.Code,
                AuthenticationErrors.UserAlreadyExists.Message
            );
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            EmailConfirmed = false,
            UserName = request.UserName,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(',', result.Errors.Select(x => x.Code)));
        }

        await userManager.AddToRoleAsync(user, "User");
        await userManager.UpdateAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwtService.GenerateAccessToken(user, roles.FirstOrDefault());
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = timeProvider
            .GetUtcNow()
            .AddDays(options.Value.RefreshTokenExpirationDays)
            .UtcDateTime;

        await userManager.UpdateAsync(user);

        return new RegisterCommandResponse(accessToken, refreshToken);
    }
}
