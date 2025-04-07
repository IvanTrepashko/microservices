using AutoMapper;
using ClientService.API.ApiModels;
using ClientService.Application.Commands.Clients;
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
        var userId = HttpContext.GetUserId();
        var query = new GetProfileInfoQuery(userId);

        var result = await sender.Send(query);

        var response = mapper.Map<ProfileInfoApiResponse>(result);

        return Ok(response);
    }

    [HttpPost("profile")]
    public async Task<IActionResult> CreateClientProfile(CreateClientRequest request)
    {
        var command = new CreateClientProfileCommand(
            HttpContext.GetUserId(),
            Email.Create(request.Email.Value),
            request.Email,
            request.PhoneNumber,
            request.Address,
            request.BirthDate,
            request.Gender
        );
        await sender.Send(command);
        return Ok();
    }
}
