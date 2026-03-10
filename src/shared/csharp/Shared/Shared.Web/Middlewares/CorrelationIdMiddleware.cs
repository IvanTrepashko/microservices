using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Shared.Web.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    private const string CorrelationId = "CorrelationId";

    public Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var value))
        {
            value = Guid.NewGuid().ToString();
        }

        context.Items[CorrelationId] = value;
        LogContext.PushProperty(CorrelationId, value);
        context.Response.Headers[CorrelationIdHeaderName] = value;

        return next(context);
    }
}
