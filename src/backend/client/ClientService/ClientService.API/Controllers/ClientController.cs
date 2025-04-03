using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers;

[Route("api/client")]
[ApiController]
[Authorize]
public class ClientController(ISender sender, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfileInfo()
    {
        var query = new GetProfileInfoQuery(HttpContext.GetUserId());
        var result = await sender.Send(query);
        var response = mapper.Map<ProfileInfoApiResponse>(result);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient(CreateClientRequest request)
    {
        var command = mapper.Map<CreateClientCommand>(request);
        var result = await sender.Send(command);
        var response = mapper.Map<CreateClientResponse>(result);
        return Ok(response);
    }
}
