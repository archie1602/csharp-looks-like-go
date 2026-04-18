using System.Diagnostics;

namespace middleware;

class RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var start = Stopwatch.GetTimestamp();
        await next(context);
        var elapsed = Stopwatch.GetElapsedTime(start);

        logger.LogInformation(
            "{Method} {Path} -> {Status} ({ElapsedMs:0.00} ms)",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            elapsed.TotalMilliseconds);
    }
}
