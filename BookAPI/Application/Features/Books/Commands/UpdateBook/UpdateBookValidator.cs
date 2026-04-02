using FluentValidation;

namespace Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Isbn).NotEmpty().MinimumLength(10).MaximumLength(17);
        RuleFor(x => x.PublishedOn).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}