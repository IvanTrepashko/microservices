using System.Text.RegularExpressions;
using ApiGateway.Ocelot.Configuration;
using ApiGateway.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Ocelot.Authentication.Middleware;
using Ocelot.Authorization.Middleware;
using Ocelot.Cache.Middleware;
using Ocelot.Claims.Middleware;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamPathManipulation.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Headers.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.Middleware;
using Ocelot.QueryStrings.Middleware;
using Ocelot.Request.Middleware;
using Ocelot.Requester.Middleware;
using Ocelot.RequestId.Middleware;

namespace ApiGateway.Extensions;

public static partial class OcelotExtensions
{
    private const char LeftBracket = '{';
    private const char RightBracket = '}';
    private const string RootDirectory = "ocelot";
    private const string PrimaryConfigFile = "ocelot.json";
    private const string GlobalConfigFile = "ocelot.global.json";

    private const string SubConfigPattern = @"^ocelot\.(.*?)\.json$";

    public static IServiceCollection AddConfigureOcelot(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment
    )
    {
        var ocelotConfigurationBuilder = new ConfigurationBuilder().AddOcelotCustom(
            "routes",
            webHostEnvironment
        );

        var directory = Path.GetDirectoryName(AppContext.BaseDirectory);
        const string fileName = "ocelot.json";

        File.Copy(
            Path.Combine(webHostEnvironment.ContentRootPath, fileName),
            Path.Combine(directory, fileName),
            overwrite: true
        );

        var ocelotConfiguration = ocelotConfigurationBuilder.Build();

        services.Configure<CustomFileConfiguration>(ocelotConfiguration);
        services.AddSingleton<IOptionsMonitor<FileConfiguration>>(p =>
            p.GetRequiredService<IOptionsMonitor<CustomFileConfiguration>>()
        );
        var routeOptions = ocelotConfiguration.GetSection("Routes");
        services.Configure<List<RouteOptions>>(routeOptions);

        services.AddOcelot(ocelotConfiguration);

        services.Configure<OcelotGlobalConfiguration>(
            configuration.GetSection(nameof(FileConfiguration.GlobalConfiguration))
        );

        services.PostConfigure<CustomFileConfiguration>(fileConfiguration =>
        {
            var ocelotOptions = new OcelotGlobalConfiguration();
            var ocelotConfigurationSection = configuration.GetSection(
                nameof(FileConfiguration.GlobalConfiguration)
            );
            ocelotConfigurationSection.Bind(ocelotOptions);

            if (!(ocelotOptions.ClusterServiceNames?.Any() ?? false))
            {
                return;
            }

            foreach (var route in fileConfiguration.Routes)
            {
                foreach (var pair in route.DownstreamHostAndPorts)
                {
                    ConfigureRoute(route, pair, ocelotOptions);
                }
            }
        });

        return services;
    }

    public static IApplicationBuilder UseOcelotGateway(
        this IApplicationBuilder app,
        Action<OcelotPipelineConfiguration> action = null
    )
    {
        var config = new OcelotPipelineConfiguration();
        action?.Invoke(config);

        app.UseOcelot(pipelineConfiguration =>
            {
                pipelineConfiguration.MapWhenOcelotPipeline.Add(
                    (_) => true,
                    (a) => UseInternalPipeline(a, config)
                );
            })
            .GetAwaiter()
            .GetResult();
        return app;
    }

    private static void UseInternalPipeline(
        IApplicationBuilder app,
        OcelotPipelineConfiguration pipelineConfiguration
    )
    {
        app.UseHttpHeadersTransformationMiddleware();
        app.UseDownstreamRequestInitialiser();
        app.UseRequestIdMiddleware();
        app.UseIfNotNull(pipelineConfiguration.PreAuthorizationMiddleware);
        if (pipelineConfiguration.AuthorizationMiddleware == null)
            app.UseAuthorizationMiddleware();
        else
            app.Use(pipelineConfiguration.AuthorizationMiddleware);
        app.UseIfNotNull(pipelineConfiguration.PreAuthenticationMiddleware);
        if (pipelineConfiguration.AuthenticationMiddleware == null)
            app.UseAuthenticationMiddleware();
        else
            app.Use(pipelineConfiguration.AuthenticationMiddleware);
        app.UseClaimsToClaimsMiddleware();
        app.UseClaimsToHeadersMiddleware();
        app.UseClaimsToQueryStringMiddleware();
        app.UseClaimsToDownstreamPathMiddleware();
        app.UseLoadBalancingMiddleware();
        app.UseDownstreamUrlCreatorMiddleware();
        app.UseOutputCacheMiddleware();
        app.UseHttpRequesterMiddleware();
    }

    #region Helper methods

    private static void UseIfNotNull(
        this IApplicationBuilder builder,
        Func<HttpContext, Func<Task>, Task> middleware
    )
    {
        if (middleware == null)
            return;
        builder.Use(middleware);
    }

    private static void ConfigureRoute(
        FileRoute route,
        FileHostAndPort hostAndPort,
        OcelotGlobalConfiguration ocelotOptions
    )
    {
        var host = hostAndPort.Host;

        if (
            string.IsNullOrEmpty(host)
            || !host.Contains(LeftBracket)
            || !host.EndsWith(RightBracket)
        )
            return;

        var match = ServiceRegex().Match(host);

        if (match.Success)
        {
            var serviceName = match.Groups["service"].Value;

            if (!ocelotOptions.ClusterServiceNames.TryGetValue(serviceName, out var serviceUrl))
            {
                throw new Exception($"Service url for {hostAndPort.Host} not found.");
            }

            var (scheme, hostName, port) = GetParts(serviceUrl);

            (hostAndPort.Host, hostAndPort.Port) = (hostName, port);

            if (string.IsNullOrEmpty(route.DownstreamScheme))
            {
                route.DownstreamScheme = scheme;
            }
        }
    }

    private static (string scheme, string host, int port) GetParts(string uri)
    {
        var url = new Uri(uri);
        return (url.Scheme, url.Host, url.Port);
    }

    private static IConfigurationBuilder AddOcelotCustom(
        this IConfigurationBuilder builder,
        string folder,
        IWebHostEnvironment env
    )
    {
        string excludeConfigName =
            env?.EnvironmentName != null ? $"ocelot.{env.EnvironmentName}.json" : string.Empty;

        var envFileName = $"ocelot.global.{env.EnvironmentName}.json";

        var reg = new Regex(SubConfigPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _ = new List<FileInfo>();

        List<FileInfo> files = LoadCustomRoutes(folder, reg, excludeConfigName, envFileName);

        var globalConfig = JObject.Parse("{}");
        var aggregatesConfig = JArray.Parse("[]");
        var routes = JArray.Parse("[]");
        var swaggerEndpoints = JArray.Parse("[]");

        foreach (var file in files)
        {
            if (
                files.Count > 1
                && file.Name.Equals(PrimaryConfigFile, StringComparison.OrdinalIgnoreCase)
            )
            {
                continue;
            }

            var lines = File.ReadAllText(file.FullName);

            var config = JObject.Parse(lines);

            if (file.Name.Equals(GlobalConfigFile, StringComparison.OrdinalIgnoreCase))
            {
                globalConfig = (JObject)config[nameof(FileConfiguration.GlobalConfiguration)];
            }

            AddRange(aggregatesConfig, "Aggregates");
            AddRange(routes, "Routes");

            void AddRange(JArray obj, string key)
            {
                if (config[key] is not JArray arrayToAdd)
                    return;
                foreach (var item in arrayToAdd)
                {
                    obj.Add(item);
                }
            }
        }

        var obj = JObject.Parse("{}");
        obj["GlobalConfiguration"] = globalConfig;
        obj["Aggregates"] = aggregatesConfig;
        obj["Routes"] = routes;

        var json = obj.ToString();

        File.WriteAllText(PrimaryConfigFile, json);

        builder.AddJsonFile(PrimaryConfigFile, false, false);

        return builder;
    }

    private static List<FileInfo> LoadCustomRoutes(
        string folder,
        Regex reg,
        string excludeConfigName,
        string envFileName
    )
    {
        List<FileInfo> files;
        if (string.IsNullOrWhiteSpace(folder))
        {
            files = new DirectoryInfo(RootDirectory)
                .EnumerateFiles("*", SearchOption.AllDirectories)
                .Where(fi => reg.IsMatch(fi.Name) && (fi.Name != excludeConfigName))
                .Select(fi =>
                    (
                        fi,
                        global: fi.Name.Equals(GlobalConfigFile),
                        envFileName: fi.Name.Equals(envFileName)
                    )
                )
                .OrderBy(c => (c.global || c.envFileName, c.envFileName))
                .Select(c => c.fi)
                .ToList();
        }
        else
        {
            var globalFiles = new DirectoryInfo(RootDirectory)
                .EnumerateFiles()
                .Select(fi =>
                    (
                        fi,
                        global: fi.Name.Equals(GlobalConfigFile),
                        envFileName: fi.Name.Equals(envFileName)
                    )
                )
                .OrderBy(c => (c.global || c.envFileName, c.envFileName))
                .Select(c => c.fi)
                .ToList();

            files = new DirectoryInfo(Path.Combine(RootDirectory, folder))
                .EnumerateFiles()
                .Where(fi => reg.IsMatch(fi.Name) && (fi.Name != excludeConfigName))
                .Select(c => c)
                .ToList();

            files.AddRange(globalFiles);
        }

        return files;
    }

    [GeneratedRegex(
        @"^{service:(?<service>\w+)}$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        "en-US"
    )]
    private static partial Regex ServiceRegex();

    #endregion Helper methods
}