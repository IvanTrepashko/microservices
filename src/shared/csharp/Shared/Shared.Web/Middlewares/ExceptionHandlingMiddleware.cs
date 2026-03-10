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

            var (errorCode, errorMessage) = ex is Shared.Core.Exceptions.ApplicationException appEx
                ? (appEx.ErrorCode, appEx.ErrorMessage)
                : ((string?)null, "An unexpected error occurred");

            var response = new ProblemResponse
            {
                Title = title,
                Status = (int)statusCode,
                Code = errorCode,
                Message = errorMessage,
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
        public string? Title { get; init; }
        public int Status { get; init; }
        public string? Code { get; set; }
        public string Message { get; set; }
        public string? Instance { get; init; }
        public string? CorrelationId { get; init; }
    }
}
