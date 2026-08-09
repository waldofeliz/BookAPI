using System.Threading.RateLimiting;
using Api.Configuration;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Extensions;

public static class RateLimitingExtensions
{
    public const string AuthPolicyName = "auth";

    public static IServiceCollection AddBookApiRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration.GetSection(RateLimitingOptions.SectionName).Get<RateLimitingOptions>()
                      ?? new RateLimitingOptions();

        services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            limiterOptions.AddPolicy(AuthPolicyName, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = options.Auth.PermitLimit,
                        Window = TimeSpan.FromSeconds(options.Auth.WindowSeconds),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));
        });

        return services;
    }
}
