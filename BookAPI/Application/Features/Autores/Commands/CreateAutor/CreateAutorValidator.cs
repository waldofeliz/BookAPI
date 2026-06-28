using FluentValidation;

namespace Application.Features.Autores.Commands.CreateAutor;

public sealed class CreateAutorValidator : AbstractValidator<CreateAutorCommand>
{
    public CreateAutorValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Apellido).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cumpleanio).NotEmpty();
        RuleFor(x => x.CreadoPor).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Biografia).MaximumLength(2000).When(x => x.Biografia is not null);
        RuleFor(x => x.Nacionalidad).MaximumLength(100).When(x => x.Nacionalidad is not null);
    }
}