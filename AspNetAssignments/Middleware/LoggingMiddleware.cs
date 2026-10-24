using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNetAssignments.Middleware;

/// <summary>
/// Проміжне ПЗ для базового логування методу, шляху та статусу відповіді HTTP-запитів
/// </summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Логує інформацію про початок та завершення обробки HTTP-запиту
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("[REQUEST] Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
        
        await _next(context);

        _logger.LogInformation("[RESPONSE] Finished handling request. Status: {StatusCode}", context.Response.StatusCode);
    }
}
