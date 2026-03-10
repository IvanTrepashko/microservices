using AuthService.API.ApiModels;
using AuthService.Application.Commands.Authentication;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender, IMapper mapper) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestApiModel request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await sender.Send(command, HttpContext.RequestAborted);
        var response = mapper.Map<LoginResponseApiModel>(result);

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponseApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestApiModel request)
    {
        var command = new RegisterCommand(request.Email, request.Password, request.UserName);
        var result = await sender.Send(command, HttpContext.RequestAborted);
        var response = mapper.Map<RegisterResponseApiModel>(result);

        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestApiModel request)
    {
        var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);
        var result = await sender.Send(command, HttpContext.RequestAborted);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand(User);
        await sender.Send(command, HttpContext.RequestAborted);
        return Ok();
    }
}
