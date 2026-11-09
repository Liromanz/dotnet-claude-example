using System.Net;
using MetricBoard.Common.Application;
using MetricBoard.Common.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricBoard.Common.Api.Middleware;

/// <summary>
/// Translates unhandled exceptions into RFC 7807 ProblemDetails. DomainException (a rule
/// violation raised inside an aggregate) maps to 400; everything else is a 500 — handlers
/// are expected to only ever throw DomainException for anything the client could fix.
/// </summary>
public sealed class ProblemDetailsExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Domain rule violated", ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, "Not found", ex.Message);
        }
        catch (Exception ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Unexpected error", ex.Message);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode status, string title, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)status;

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path,
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}

public static class ProblemDetailsExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseProblemDetailsExceptionHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
}
