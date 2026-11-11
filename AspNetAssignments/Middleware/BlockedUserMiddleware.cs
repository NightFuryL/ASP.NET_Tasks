using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAssignments.Middleware;

public class BlockedUserMiddleware
{
    private readonly RequestDelegate _next;

    private static readonly HashSet<Guid> BlockedUserIds = new()
    {
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Guid.Parse("33333333-3333-3333-3333-333333333333")
    };

    public BlockedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) &&
            Guid.TryParse(userIdHeader, out var userId) &&
            BlockedUserIds.Contains(userId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "User Blocked",
                Detail = "Ваш акаунт заблоковано за порушення правил сервісу.",
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
            return;
        }

        await _next(context);
    }
}

public static class BlockedUserMiddlewareExtensions
{
    public static IApplicationBuilder UseBlockedUser(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockedUserMiddleware>();
    }
}
