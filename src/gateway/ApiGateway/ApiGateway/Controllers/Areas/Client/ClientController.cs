using ApiGateway.Controllers.Areas.Client.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.Areas.Client;

[ApiController]
[Route("api/client")]
public class ClientController : ControllerBase
{
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ProfileInfoApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileInfo() => Ok();

    [HttpPost("profile")]
    public async Task<IActionResult> CreateClientProfile(CreateClientRequest request) => Ok();
}