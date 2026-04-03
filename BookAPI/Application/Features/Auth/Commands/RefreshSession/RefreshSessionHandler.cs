using Application.Abstractions.Security;
using Application.Features.Auth.Dtos;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshSession;

public sealed class RefreshSessionHandler : IRequestHandler<RefreshSessionCommand, AuthTokensDto>
{
    private readonly IAuthService _auth;

    public RefreshSessionHandler(IAuthService auth) => _auth = auth;

    public Task<AuthTokensDto> Handle(RefreshSessionCommand request, CancellationToken ct)
        => _auth.RefreshAsync(request.RefreshToken, request.IpAddress, ct);
}
