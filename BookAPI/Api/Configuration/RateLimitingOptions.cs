namespace Api.Configuration;

public sealed class RateLimitingOptions
{
    public const string SectionName = "RateLimiting";

    public AuthRateLimitOptions Auth { get; init; } = new();
}

public sealed class AuthRateLimitOptions
{
    public int PermitLimit { get; init; } = 10;
    public int WindowSeconds { get; init; } = 60;
}
