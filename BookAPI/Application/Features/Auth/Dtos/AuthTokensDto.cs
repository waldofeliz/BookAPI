namespace Application.Features.Auth.Dtos;

public sealed record AuthTokensDto(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);
