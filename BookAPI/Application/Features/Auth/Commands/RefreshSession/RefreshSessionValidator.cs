using FluentValidation;

namespace Application.Features.Auth.Commands.RefreshSession;

public sealed class RefreshSessionValidator : AbstractValidator<RefreshSessionCommand>
{
    public RefreshSessionValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
