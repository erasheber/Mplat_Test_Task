using Microsoft.Extensions.Options;

namespace Dotnet_Test_Task.Api.Security;

public sealed class ApiKeyOptions
{
    public string ApiKey { get; init; } = "";
}

public sealed class ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeyOptions> options)
{
    private readonly ApiKeyOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var provided) ||
            string.IsNullOrWhiteSpace(_options.ApiKey) ||
            !string.Equals(provided.ToString(), _options.ApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing or invalid X-Api-Key header.");
            return;
        }

        await next(context);
    }
}