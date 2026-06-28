using FluentValidation;

namespace Application.Features.Autores.Queries.GetAutorPorId;

public sealed class GetAutorPorIdValidator : AbstractValidator<GetAutorPorIdQuery>
{
    public GetAutorPorIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
