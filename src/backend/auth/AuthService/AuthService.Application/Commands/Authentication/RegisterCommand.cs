using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.DbContexts;
using AuthService.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Commands.Authentication;

public record RegisterCommand(string Email, string Password, string UserName)
    : IRequest<RegisterCommandResponse>;

public record RegisterCommandResponse(string AccessToken, string RefreshToken);

public class RegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtService jwtService,
    AuthContext authContext
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
            // todo: add error handling
            throw new Exception("User already exists");
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
            // todo: add error handling
            throw new Exception("Failed to create user");
        }

        await userManager.AddToRoleAsync(user, "User");
        await userManager.UpdateAsync(user);

        // Generate tokens
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwtService.GenerateAccessToken(user, roles);
        var refreshToken = jwtService.GenerateRefreshToken();

        // Store refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return new RegisterCommandResponse(accessToken, refreshToken);
    }
}
