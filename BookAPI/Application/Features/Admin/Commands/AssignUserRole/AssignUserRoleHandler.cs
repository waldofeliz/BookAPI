using Application.Abstractions.Security;
using MediatR;

namespace Application.Features.Admin.Commands.AssignUserRole;

public sealed class AssignUserRoleHandler : IRequestHandler<AssignUserRoleCommand>
{
    private readonly IUserRoleService _userRoles;

    public AssignUserRoleHandler(IUserRoleService userRoles) => _userRoles = userRoles;

    public Task Handle(AssignUserRoleCommand request, CancellationToken ct)
        => _userRoles.AssignCatalogRoleAsync(request.Email, request.Role, ct);
}
