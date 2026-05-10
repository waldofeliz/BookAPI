using Application.Features.Books.Dtos;
using MediatR;

namespace Application.Features.Books.Commands.UpdateBook;

public sealed record UpdateBookCommand(
    Guid Id,
    string Title,
    string Isbn,
    DateTime PublishedOn,
    string? Description,
    string? ModifiedBy,
    bool State
) : IRequest<BookDto>;