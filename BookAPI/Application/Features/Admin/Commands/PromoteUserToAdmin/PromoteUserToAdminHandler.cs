using Application.Abstractions.Security;
using MediatR;

namespace Application.Features.Admin.Commands.PromoteUserToAdmin;

public sealed class PromoteUserToAdminHandler : IRequestHandler<PromoteUserToAdminCommand>
{
    private readonly IUserRoleService _userRoles;

    public PromoteUserToAdminHandler(IUserRoleService userRoles) => _userRoles = userRoles;

    public Task Handle(PromoteUserToAdminCommand request, CancellationToken ct)
        => _userRoles.PromoteToAdminAsync(request.Email, ct);
}
