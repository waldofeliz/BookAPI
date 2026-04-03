namespace Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = default!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? CreatedByIp { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? RevokedByIp { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    private RefreshToken() { }

    public RefreshToken(Guid userId, string tokenHash, DateTime expiresAt, DateTime createdAt, string? createdByIp)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        CreatedByIp = createdByIp;
    }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt.HasValue;

    public void Revoke(string? ip, string? replacedByHash)
    {
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = ip;
        ReplacedByTokenHash = replacedByHash;
    }
}
