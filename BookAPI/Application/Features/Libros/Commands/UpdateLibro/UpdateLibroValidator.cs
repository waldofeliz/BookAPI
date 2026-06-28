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
        RuleFor(x => x.ModificadoPor).MaximumLength(100).When(x => x.ModificadoPor is not null);
        RuleFor(x => x.Descripcion).MaximumLength(2000).When(x => x.Descripcion is not null);
        RuleFor(x => x.SubTitulo).MaximumLength(250).When(x => x.SubTitulo is not null);
        RuleFor(x => x.CoverImageUrl).MaximumLength(500).When(x => x.CoverImageUrl is not null);
        RuleFor(x => x.Edicion).MaximumLength(50).When(x => x.Edicion is not null);
        RuleFor(x => x.Lenguaje).MaximumLength(50).When(x => x.Lenguaje is not null);
        RuleFor(x => x.Paginas).GreaterThan(0);
    }
}