using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("test");
    }
}