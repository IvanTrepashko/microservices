using Ocelot.Configuration.File;

namespace ApiGateway.Ocelot.Configuration;

public class CustomFileConfiguration : FileConfiguration
{
    public CustomFileConfiguration()
    {
        GlobalConfiguration = new CustomOcelotGlobalConfiguration();
        Routes = new List<FileRoute>();
    }
}