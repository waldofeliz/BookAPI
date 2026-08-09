using FluentValidation;

namespace Application.Features.Admin.Commands.PromoteUserToAdmin;

public sealed class PromoteUserToAdminValidator : AbstractValidator<PromoteUserToAdminCommand>
{
    public PromoteUserToAdminValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
    }
}
