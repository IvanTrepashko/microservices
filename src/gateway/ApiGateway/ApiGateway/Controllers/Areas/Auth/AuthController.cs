using ApiGateway.Controllers.Areas.Auth.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.Areas.Auth;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestApiModel request) => Ok();

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponseApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestApiModel request) => Ok();
}