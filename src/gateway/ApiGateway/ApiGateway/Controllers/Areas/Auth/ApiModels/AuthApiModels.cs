namespace ApiGateway.Controllers.Areas.Auth.ApiModels;

public record LoginRequestApiModel(string Email, string Password);

public record LoginResponseApiModel(string AccessToken, string RefreshToken);

public record RegisterRequestApiModel(string Email, string Password, string UserName);

public record RegisterResponseApiModel(string AccessToken, string RefreshToken);

public record RefreshTokenRequestApiModel(string AccessToken, string RefreshToken);