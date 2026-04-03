using Application.Features.Auth.Dtos;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshSession;

public sealed record RefreshSessionCommand(string RefreshToken, string? IpAddress) : IRequest<AuthTokensDto>;
