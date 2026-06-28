using FluentValidation;

namespace Application.Features.Autores.Commands.DeleteAutor;

public sealed class DeleteAutorValidator : AbstractValidator<DeleteAutorCommand>
{
    public DeleteAutorValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
