using FluentValidation;

namespace Application.Features.Libros.Queries.GetLibroPorId;

public sealed class GetLibroPorIdValidator : AbstractValidator<GetLibroPorIdQuery>
{
    public GetLibroPorIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
