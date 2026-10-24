using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNetAssignments.Middleware;

/// <summary>
/// Проміжне ПЗ для логування тривалості виконання HTTP-запитів за допомогою Stopwatch
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Обробляє HTTP-запит та фіксує точний час його виконання
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation("${Request method} {uri} => {statusCode} executed in {elapsedMilliseconds}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            elapsedMilliseconds);
    }
}

/// <summary>
/// Методи розширення для зручної реєстрації RequestLoggingMiddleware у конвеєрі
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    /// <summary>
    /// Підключає логування тривалості запитів до HTTP конвеєра
    /// </summary>
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
