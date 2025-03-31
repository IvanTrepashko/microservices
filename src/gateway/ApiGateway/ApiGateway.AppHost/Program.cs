var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiGateway>("apigateway");

await builder.Build().RunAsync();