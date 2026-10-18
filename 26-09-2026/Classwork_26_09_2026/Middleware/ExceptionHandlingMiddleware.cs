using Classwork_26_09_2026.Exceptions;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace Classwork_26_09_2026.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next = null!;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = requestDelegate;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        var (statusCode, message) = ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, $"Error server: {ex.Message}"),
        };
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = context.Response.StatusCode,
            error = message,
            timestamp = DateTime.UtcNow,
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
