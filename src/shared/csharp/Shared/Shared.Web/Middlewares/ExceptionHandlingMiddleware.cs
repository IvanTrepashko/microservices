using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Core.Exceptions;

namespace Shared.Web.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private static readonly ILogger Logger = Log.ForContext<ExceptionHandlingMiddleware>();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, title) = MapException(ex);

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                Logger.Error(ex, "Unhandled exception");
            }
            else
            {
                Logger.Warning(
                    "Request failed with {StatusCode}: {Message}",
                    (int)statusCode,
                    ex.Message
                );
            }

            var correlationId = context.Items.TryGetValue("CorrelationId", out var value)
                ? value?.ToString()
                : null;

            var response = new ProblemResponse
            {
                Type = $"https://httpstatuses.com/{(int)statusCode}",
                Title = title,
                Status = (int)statusCode,
                Detail = ex.Message,
                Instance = context.Request.Path,
                CorrelationId = correlationId,
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }

    private static (HttpStatusCode statusCode, string title) MapException(Exception exception) =>
        exception switch
        {
            BadRequestException => (HttpStatusCode.BadRequest, "Bad Request"),
            NotFoundException => (HttpStatusCode.NotFound, "Not Found"),
            UnauthorizedException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            ForbiddenException => (HttpStatusCode.Forbidden, "Forbidden"),
            NotAllowedException => (HttpStatusCode.MethodNotAllowed, "Method Not Allowed"),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error"),
        };

    private sealed class ProblemResponse
    {
        public string? Type { get; init; }
        public string? Title { get; init; }
        public int Status { get; init; }
        public string? Detail { get; init; }
        public string? Instance { get; init; }
        public string? CorrelationId { get; init; }
    }
}
