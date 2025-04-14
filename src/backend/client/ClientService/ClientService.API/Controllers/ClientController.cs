using AutoMapper;
using ClientService.API.ApiModels;
using ClientService.API.Extensions;
using ClientService.Application.Commands.Clients;
using ClientService.Application.Queries.Clients;
using ClientService.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers;

[Route("api/client")]
[ApiController]
[Authorize]
public class ClientController(ISender sender, IMapper mapper) : ControllerBase
{
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ProfileInfoApiResponse), StatusCodes.Status200OK)]
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
            Name.Create(request.Name.FirstName, request.Name.LastName),
            Email.Create(request.Email.Value),
            PhoneNumber.Create(request.PhoneNumber.Value),
            Address.Create(
                request.Address.Street,
                request.Address.City,
                request.Address.State,
                request.Address.ZipCode,
                request.Address.Country
            ),
            request.BirthDate,
            (Domain.Entities.ClientGender)request.Gender
        );
        await sender.Send(command);
        return Ok();
    }
}