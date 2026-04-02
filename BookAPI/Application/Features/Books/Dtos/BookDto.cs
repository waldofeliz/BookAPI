namespace Application.Features.Books.Dtos;

public sealed record BookDto
(
    Guid Id,
    string Title,
    string Isbn,
    DateTime PublishedOn,
    string? Description
 );