using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Infrastructure.RabbitMQ.Options;

public class RabbitMqOptions
{
    public string ConnectionString { get; set; }
    public string EndpointPrefix { get; set; }
}

public class RabbitMqOptionsConfigure : IConfigureOptions<RabbitMqOptions>
{
    private const string RabbitMqKey = "RabbitMQ";

    private readonly IConfiguration _configuration;

    public RabbitMqOptionsConfigure(IConfiguration configuration) => _configuration = configuration;

    public void Configure(RabbitMqOptions options)
    {
        _configuration.GetSection(RabbitMqKey).Bind(options);
    }
}