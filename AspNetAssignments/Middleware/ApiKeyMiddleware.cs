using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AspNetAssignments.Middleware;

/// <summary>
/// Проміжне ПЗ для авторизації запитів за допомогою фіксованого API-ключа в заголовку X-Api-Key
/// </summary>
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_HEADER_NAME = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Перевіряє наявність та валідність API ключа в заголовках HTTP-запиту
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // Пропуск Swagger документації та системних шляхів
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }

        var configuredApiKey = configuration.GetValue<string>("Authentication:ApiKey") ?? "MySecretApiKey_12345";

        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey) ||
            !string.Equals(extractedApiKey, configuredApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\":\"Недійсний або відсутній X-Api-Key заголовок.\"}");
            return;
        }

        await _next(context);
    }
}
