using FluentValidation;

namespace Application.Features.Libros.Queries.ListLibros;

public sealed class ListLibrosValidator : AbstractValidator<ListLibrosQuery>
{
    public ListLibrosValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(100).When(x => x.Search is not null);
    }
}
