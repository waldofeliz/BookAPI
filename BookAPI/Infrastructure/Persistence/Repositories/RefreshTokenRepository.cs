using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly BookDbContext _db;

    public RefreshTokenRepository(BookDbContext db) => _db = db;

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct)
        => await _db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, ct);

    public async Task AddAsync(RefreshToken token, CancellationToken ct)
        => await _db.RefreshTokens.AddAsync(token, ct);

    public void Update(RefreshToken token) => _db.RefreshTokens.Update(token);

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct)
    {
        await _db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow), ct);
    }
}
