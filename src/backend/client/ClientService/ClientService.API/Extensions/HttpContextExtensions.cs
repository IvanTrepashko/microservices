using System.Security.Claims;

namespace ClientService.API.Extensions;

public static class HttpContextExtensions
{
    public static long GetUserId(this HttpContext httpContext)
    {
        long.TryParse(httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out var userId);

        return userId;
    }
}