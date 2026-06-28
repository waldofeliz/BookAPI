using FluentValidation;

namespace Application.Features.Libros.Commands.DeleteLibro;

public sealed class DeleteLibroValidator : AbstractValidator<DeleteLibroCommand>
{
    public DeleteLibroValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
