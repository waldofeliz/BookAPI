namespace Api.Contracts.Books;

public sealed record CreateBookRequest
(
    string Title,
    string Isbn,
    DateTime PublishedOn,
    string? Description
);