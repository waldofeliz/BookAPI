using Application.Abstractions.Security;
using Application.Features.Auth.Dtos;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, AuthTokensDto>
{
    private readonly IAuthService _auth;

    public LoginHandler(IAuthService auth) => _auth = auth;

    public Task<AuthTokensDto> Handle(LoginCommand request, CancellationToken ct)
        => _auth.LoginAsync(request.Email, request.Password, request.IpAddress, ct);
}
