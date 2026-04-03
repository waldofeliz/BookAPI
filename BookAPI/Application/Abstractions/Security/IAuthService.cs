using Application.Features.Auth.Dtos;

namespace Application.Abstractions.Security;

public interface IAuthService
{
    Task<AuthTokensDto> RegisterAsync(string email, string password, string? ipAddress, CancellationToken ct);

    Task<AuthTokensDto> LoginAsync(string email, string password, string? ipAddress, CancellationToken ct);

    Task<AuthTokensDto> RefreshAsync(string refreshToken, string? ipAddress, CancellationToken ct);
}
