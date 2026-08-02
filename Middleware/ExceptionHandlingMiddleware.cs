using System.Net;
using System.Text.Json;
using TaskManagerApi.Exceptions;

namespace TaskManagerApi.Middleware;

/// <summary>
/// Catches domain exceptions thrown by the service layer and converts them
/// into consistent JSON error responses. This is the only place in the app
/// that maps exceptions to HTTP status codes, so controllers and services
/// stay free of try/catch blocks and status-code decisions.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (TaskNotFoundException ex)
        {
            await WriteError(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (InvalidTaskDataException ex)
        {
            await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, string detail)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var payload = JsonSerializer.Serialize(new { detail });
        await context.Response.WriteAsync(payload);
    }
}
