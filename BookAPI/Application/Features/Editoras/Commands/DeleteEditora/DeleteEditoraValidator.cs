using FluentValidation;

namespace Application.Features.Editoras.Commands.DeleteEditora;

public sealed class DeleteEditoraValidator : AbstractValidator<DeleteEditoraCommand>
{
    public DeleteEditoraValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
