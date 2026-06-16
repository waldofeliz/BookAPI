using FluentValidation;

namespace Application.Features.Autores.Commands.CreateAutor;

public sealed class CreateAutorValidator : AbstractValidator<CreateAutorCommand>
{
    public CreateAutorValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Apellido).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cumpleanio).NotEmpty().NotNull();
    }
}