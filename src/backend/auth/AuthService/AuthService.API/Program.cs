using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthService.API.Extensions;
using AuthService.API.Mappings;
using AuthService.Application.Extensions;
using AuthService.Application.Options;
using AuthService.Application.Services.Abstractions;
using AuthService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shared.Web.Extensions;

namespace AuthService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

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

        builder
            .Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey)
                    ),
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var blacklistService =
                            context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();

                        var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                        if (
                            jti != null
                            && await blacklistService.IsInBlacklistAsync(
                                jti,
                                context.HttpContext.RequestAborted
                            )
                        )
                        {
                            context.Fail("Token has been revoked");
                        }
                    },
                };
            });

        var app = builder.Build();

        app.MigrateDatabaseAsync().Wait();

        app.UseSerilogRequestLogging();

        app.AddCorrelationId();

        app.MapDefaultEndpoints();

        app.UseCors();

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
