using FluentValidation;

namespace Application.Features.Libros.Commands.UpdateLibro;

public sealed class UpdateLibroValidator : AbstractValidator<UpdateLibroCommand>
{
    public UpdateLibroValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MinimumLength(10).MaximumLength(17);
        RuleFor(x => x.PublicadoEn).NotEmpty();
        RuleFor(x => x.Descripcion).MaximumLength(2000);
    }
}