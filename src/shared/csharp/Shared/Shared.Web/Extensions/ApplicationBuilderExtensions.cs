using Microsoft.AspNetCore.Builder;
using Shared.Web.Middlewares;

namespace Shared.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddCorrelationId(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
