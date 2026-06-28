using FluentValidation;

namespace Application.Features.Autores.Commands.UpdateAutor;

public sealed class UpdateAutorValidator : AbstractValidator<UpdateAutorCommand>
{
    public UpdateAutorValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Apellido).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cumpleanio).NotEmpty();
        RuleFor(x => x.ModificadoPor).MaximumLength(100).When(x => x.ModificadoPor is not null);
        RuleFor(x => x.Biografia).MaximumLength(2000).When(x => x.Biografia is not null);
        RuleFor(x => x.Nacionalidad).MaximumLength(100).When(x => x.Nacionalidad is not null);
    }
}