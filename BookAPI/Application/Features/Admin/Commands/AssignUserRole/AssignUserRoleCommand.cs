using MediatR;

namespace Application.Features.Admin.Commands.AssignUserRole;

public sealed record AssignUserRoleCommand(string Email, string Role) : IRequest;
