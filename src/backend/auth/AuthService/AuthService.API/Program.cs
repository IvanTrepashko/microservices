using AuthService.API.Mappings;
using AuthService.Application.Extensions;
using AuthService.Infrastructure.Extensions;
using AuthService.API.Extensions;

namespace AuthService.API;

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

        builder.Services.ConfigureOptions();
        builder.Services.AddApplication();
        builder.Services.AddMediatr();

        builder.Services.ConfigurePostgres(builder.Configuration);
        builder.Services.AddIdentity();

        builder.Services.AddAutoMapper(x =>
        {
            x.AddProfile<MappingProfile>();
        });

        builder.Services.AddRabbitMq();

        var app = builder.Build();

        // Apply migrations in a scope
        app.MigrateDatabaseAsync().Wait();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}