using Domain.Entities;

namespace Application.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct);

    Task AddAsync(RefreshToken token, CancellationToken ct);

    void Update(RefreshToken token);

    Task RevokeAllForUserAsync(Guid userId, CancellationToken ct);
}
