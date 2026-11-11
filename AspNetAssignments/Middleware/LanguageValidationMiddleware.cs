using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAssignments.Middleware;

public class LanguageValidationMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? acceptLanguage = context.Request.Headers["Accept-Language"];

        if (string.IsNullOrWhiteSpace(acceptLanguage))
        {
            context.Items["Language"] = "uk-UA";
            await _next(context);
            return;
        }

        if (acceptLanguage.Contains("ru", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status406NotAcceptable,
                Title = "Unsupported Language",
                Detail = "Обрана мова не підтримується сервісом. Будь ласка, використовуйте uk-UA або en-US.",
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
            return;
        }

        context.Items["Language"] = acceptLanguage;
        await _next(context);
    }
}

public static class LanguageValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseLanguageValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LanguageValidationMiddleware>();
    }
}
