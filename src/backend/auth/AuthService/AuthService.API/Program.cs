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

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

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

        app.UseCors();

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