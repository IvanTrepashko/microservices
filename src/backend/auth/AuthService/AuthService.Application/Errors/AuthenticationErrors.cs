using Shared.Core.Errors;

namespace AuthService.Application.Errors;

public static class AuthenticationErrors
{
    public static ErrorPair InvalidCredentials => new("Login.InvalidCredentials", "Given credentials are invalid");

    public static ErrorPair UserAlreadyExists => new("Register.UserAlreadyExists", "User with this email already exists");
}