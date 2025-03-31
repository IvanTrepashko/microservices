using Ocelot.Configuration.File;

namespace ApiGateway.Ocelot.Configuration;

public class CustomOcelotGlobalConfiguration : FileGlobalConfiguration
{
    public IDictionary<string, string> ClusterServiceNames { get; set; }
}