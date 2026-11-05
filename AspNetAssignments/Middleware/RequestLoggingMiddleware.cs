using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Middleware;

/// <summary>
/// Проміжне ПЗ для логування тривалості виконання HTTP-запитів за допомогою Stopwatch та збереження у файл
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private static readonly SemaphoreSlim _fallbackLock = new(1, 1);

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Обробляє HTTP-запит, фіксує точний час його виконання та зберігає дані у лог-файл
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IFileLoggerService? fileLogger = null)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        var message = $"Request {context.Request.Method} {context.Request.Path} completed with status {context.Response.StatusCode}";

        _logger.LogInformation("${Request method} {uri} => {statusCode} executed in {elapsedMilliseconds}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            elapsedMilliseconds);

        if (fileLogger != null)
        {
            await fileLogger.LogRequestAsync(
                context.Request.Method,
                context.Request.Path.Value ?? string.Empty,
                context.Response.StatusCode,
                elapsedMilliseconds,
                message);
        }
        else
        {
            await LogDirectlyToFileAsync(
                context.Request.Method,
                context.Request.Path.Value ?? string.Empty,
                context.Response.StatusCode,
                elapsedMilliseconds,
                message);
        }
    }

    private static async Task LogDirectlyToFileAsync(string method, string path, int statusCode, long durationMs, string message)
    {
        try
        {
            var logsDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
            var filePath = Path.Combine(logsDir, "requests.log");

            var logEntry = new
            {
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                method = method,
                path = path,
                statusCode = statusCode,
                durationMs = durationMs,
                message = message
            };

            var jsonLine = JsonSerializer.Serialize(logEntry);

            await _fallbackLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(filePath, jsonLine + Environment.NewLine);
            }
            finally
            {
                _fallbackLock.Release();
            }
        }
        catch
        {
            // Не блокуємо конвеєр обробки у випадку помилки файлового виводу
        }
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
