using Application.Features.Books.Dtos;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

public sealed record CreateBookCommand(
    string Title,
    string Isbn,
    DateTime PublishedOn,
    string? Description
) : IRequest<BookDto>;