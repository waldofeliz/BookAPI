using FluentValidation;

namespace Application.Features.Editoras.Queries.GetEditoraPorId;

public sealed class GetEditoraPorIdValidator : AbstractValidator<GetEditoraPorIdQuery>
{
    public GetEditoraPorIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
