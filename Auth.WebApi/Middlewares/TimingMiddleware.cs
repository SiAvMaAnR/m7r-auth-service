using System.Diagnostics;

namespace Auth.WebApi.Middlewares;

public class TimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TimingMiddleware> _logger;

    public TimingMiddleware(RequestDelegate next, ILogger<TimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;

        if (context.Request.RouteValues.Count != 0)
        {
            object controllerName = context.Request.RouteValues["controller"] ?? "unknown";
            object actionName = context.Request.RouteValues["action"] ?? "unknown";

            string message =
                $"Controller: {controllerName}. Action: {actionName}. Time: {elapsed.TotalMilliseconds} ms";

            _logger.LogInformation(message);
        }
    }
}
