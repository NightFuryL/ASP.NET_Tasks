namespace Classwork_26_09_2026.Middleware;
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next = null!;
    private const string API_KEY_HEADER_NAME = "X-Api-Key";
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (context.Request.Path.StartsWithSegments("/swager"))
        {
            await _next(context);
            return;
        }
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApikey))
        {           
            context.Response.StatusCode = StatusCodes.Status401Unauthorized; return;
            return;
        }
    }
}
