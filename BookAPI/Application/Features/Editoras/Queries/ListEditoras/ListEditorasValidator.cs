using FluentValidation;

namespace Application.Features.Editoras.Queries.ListEditoras;

public sealed class ListEditorasValidator : AbstractValidator<ListEditorasQuery>
{
    public ListEditorasValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.Search).MaximumLength(100).When(x => x.Search is not null);
    }
}
