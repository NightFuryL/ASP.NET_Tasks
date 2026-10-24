namespace Classwork_20_09_2026.Middleware;
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("[REQUEST] Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
        await _next(context); // Передача управління наступному middleware або обробнику запиту
        _logger.LogInformation("[RESPONSE] Finished handling request. {StatusCode}", context.Response.StatusCode);
    }
}
