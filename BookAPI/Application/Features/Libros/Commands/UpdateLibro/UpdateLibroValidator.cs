using FluentValidation;

namespace Application.Features.Libros.Commands.UpdateLibro;

public sealed class UpdateLibroValidator : AbstractValidator<UpdateLibroCommand>
{
    public UpdateLibroValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Isbn).NotEmpty().MinimumLength(10).MaximumLength(17);
        RuleFor(x => x.PublicadoEn).NotEmpty();
        RuleFor(x => x.SubTitulo).MaximumLength(250);
        RuleFor(x => x.CoverImageUrl).MaximumLength(500);
        RuleFor(x => x.Edicion).MaximumLength(50);
        RuleFor(x => x.Lenguaje).MaximumLength(50);
        RuleFor(x => x.Paginas).GreaterThan(0);
    }
}