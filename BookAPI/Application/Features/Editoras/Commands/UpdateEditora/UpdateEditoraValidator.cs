using FluentValidation;

namespace Application.Features.Editoras.Commands.UpdateEditora;

public sealed class UpdateEditoraValidator : AbstractValidator<UpdateEditoraCommand>
{
    public UpdateEditoraValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ModificadoPor).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Pais).MaximumLength(50).When(x => x.Pais is not null);
        RuleFor(x => x.Telefono).MaximumLength(20).When(x => x.Telefono is not null);
        RuleFor(x => x.Website)
            .MaximumLength(500)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("La URL del sitio web no es válida.")
            .When(x => x.Website is not null);
    }
}
