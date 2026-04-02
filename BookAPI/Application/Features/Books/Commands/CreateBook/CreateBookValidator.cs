using FluentValidation;

namespace Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MinimumLength(10).MaximumLength(17);
        RuleFor(x => x.PublishedOn).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}