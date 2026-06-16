using FluentValidation;

namespace Application.Features.Autores.Commands.UpdateAutor;

public sealed class UpdateAutorValidator : AbstractValidator<UpdateAutorCommand>
{
    public UpdateAutorValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Apellido).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cumpleanio).NotEmpty().NotNull();
    }
}