using ApiGateway.Extensions;

namespace ApiGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddConfigureOcelot(builder.Configuration, builder.Environment);

        //builder.Services.AddAuthorization();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseOcelotGateway();

        app.Run();
    }
}