using Application.Abstractions.Security;
using FluentValidation;

namespace Application.Features.Admin.Commands.AssignUserRole;

public sealed class AssignUserRoleValidator : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => AppRoles.AssignableCatalogRoles.Contains(role))
            .WithMessage($"El rol debe ser {AppRoles.Editor} o {AppRoles.Reader}.");
    }
}
