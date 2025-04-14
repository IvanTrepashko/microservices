using AuthService.Application.Options;
using Microsoft.Extensions.Options;

namespace AuthService.API.Configuration;

public class JwtOptionsConfigure(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection("JwtOptions").Bind(options);
    }
}