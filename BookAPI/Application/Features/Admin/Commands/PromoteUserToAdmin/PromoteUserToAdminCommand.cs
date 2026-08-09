using MediatR;

namespace Application.Features.Admin.Commands.PromoteUserToAdmin;

public sealed record PromoteUserToAdminCommand(string Email) : IRequest;
