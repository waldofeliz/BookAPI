using Application.Abstractions.Security;
using Application.Features.Auth.Dtos;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public sealed class RegisterHandler : IRequestHandler<RegisterCommand, AuthTokensDto>
{
    private readonly IAuthService _auth;

    public RegisterHandler(IAuthService auth) => _auth = auth;

    public Task<AuthTokensDto> Handle(RegisterCommand request, CancellationToken ct)
        => _auth.RegisterAsync(request.Email, request.Password, request.IpAddress, ct);
}
