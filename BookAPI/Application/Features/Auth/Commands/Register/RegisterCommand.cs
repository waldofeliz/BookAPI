using Application.Features.Auth.Dtos;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string Email, string Password, string? IpAddress) : IRequest<AuthTokensDto>;
