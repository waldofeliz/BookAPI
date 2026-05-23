using FluentValidation;

namespace Application.Features.Libros.Commands.CreateLibro;

public sealed class CreateLibroValidator : AbstractValidator<CreateLibroCommand>
{
    public CreateLibroValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MinimumLength(10).MaximumLength(17);
        RuleFor(x => x.PublicadoEn).NotEmpty();
        RuleFor(x => x.Descripcion).MaximumLength(2000);
    }
}