using FluentValidation;

namespace Application.Features.Autores.Queries.ListAutores;

public sealed class ListAutoresValidator : AbstractValidator<ListAutoresQuery>
{
    public ListAutoresValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(100).When(x => x.Search is not null);
    }
}
