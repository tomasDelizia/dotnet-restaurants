
using System.Diagnostics;

namespace Restaurants.API.Middleware;

public class TimeLoggingMiddleware(ILogger<TimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        if (elapsedMilliseconds < 4000) return;
        var httpVerb = context.Request.Method;
        var requestPath = context.Request.Path;
        logger.LogWarning(
            "[{Verb}] Request at {Path} took {ElapsedMilliseconds} ms",
            httpVerb,
            requestPath,
            elapsedMilliseconds);
    }
}
