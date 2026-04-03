using Application.Abstractions.Persistence;
using Application.Abstractions.Security;
using Application.Features.Auth.Dtos;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Security;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly JwtTokenService _jwt;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IUnitOfWork _uow;
    private readonly JwtOptions _jwtOpt;

    public AuthService(
        UserManager<ApplicationUser> users,
        SignInManager<ApplicationUser> signIn,
        JwtTokenService jwt,
        IRefreshTokenRepository refreshTokens,
        IUnitOfWork uow,
        IOptions<JwtOptions> jwtOpt)
    {
        _users = users;
        _signIn = signIn;
        _jwt = jwt;
        _refreshTokens = refreshTokens;
        _uow = uow;
        _jwtOpt = jwtOpt.Value;
    }

    public async Task<AuthTokensDto> RegisterAsync(string email, string password, string? ipAddress, CancellationToken ct)
    {
        var user = new ApplicationUser
        {
            UserName = email.Trim(),
            Email = email.Trim(),
            EmailConfirmed = true
        };

        var result = await _users.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var msg = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(msg) ? "No se pudo registrar el usuario." : msg);
        }

        return await IssueTokensAsync(user, ipAddress, ct);
    }

    public async Task<AuthTokensDto> LoginAsync(string email, string password, string? ipAddress, CancellationToken ct)
    {
        var user = await _users.FindByEmailAsync(email.Trim());
        if (user is null)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (await _users.IsLockedOutAsync(user))
            throw new UnauthorizedAccessException("La cuenta está bloqueada temporalmente.");

        var signIn = await _signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (!signIn.Succeeded)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        return await IssueTokensAsync(user, ipAddress, ct);
    }

    public async Task<AuthTokensDto> RefreshAsync(string refreshToken, string? ipAddress, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new UnauthorizedAccessException("Token de actualización inválido.");

        var hash = TokenHasher.Hash(refreshToken);
        var stored = await _refreshTokens.GetByTokenHashAsync(hash, ct);
        if (stored is null)
            throw new UnauthorizedAccessException("Token de actualización inválido.");

        if (stored.IsRevoked)
        {
            await _refreshTokens.RevokeAllForUserAsync(stored.UserId, ct);
            await _uow.SaveChangesAsync(ct);
            throw new UnauthorizedAccessException("Sesión revocada por seguridad.");
        }

        if (stored.IsExpired(DateTime.UtcNow))
            throw new UnauthorizedAccessException("Token de actualización expirado.");

        var user = await _users.FindByIdAsync(stored.UserId.ToString());
        if (user is null)
            throw new UnauthorizedAccessException("Usuario no encontrado.");

        var newPlain = JwtTokenService.GenerateRefreshTokenPlainText();
        var newHash = TokenHasher.Hash(newPlain);
        var expiresAt = DateTime.UtcNow.AddDays(_jwtOpt.RefreshTokenDays);

        stored.Revoke(ipAddress, newHash);
        _refreshTokens.Update(stored);

        var newEntity = new RefreshToken(user.Id, newHash, expiresAt, DateTime.UtcNow, ipAddress);
        await _refreshTokens.AddAsync(newEntity, ct);

        await _uow.SaveChangesAsync(ct);

        var access = _jwt.CreateAccessToken(user);
        var accessExpires = _jwt.GetAccessTokenExpiryUtc();

        return new AuthTokensDto(access, newPlain, accessExpires);
    }

    private async Task<AuthTokensDto> IssueTokensAsync(ApplicationUser user, string? ipAddress, CancellationToken ct)
    {
        var plain = JwtTokenService.GenerateRefreshTokenPlainText();
        var hash = TokenHasher.Hash(plain);
        var expiresAt = DateTime.UtcNow.AddDays(_jwtOpt.RefreshTokenDays);

        var entity = new RefreshToken(user.Id, hash, expiresAt, DateTime.UtcNow, ipAddress);
        await _refreshTokens.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        var access = _jwt.CreateAccessToken(user);
        var accessExpires = _jwt.GetAccessTokenExpiryUtc();

        return new AuthTokensDto(access, plain, accessExpires);
    }
}
