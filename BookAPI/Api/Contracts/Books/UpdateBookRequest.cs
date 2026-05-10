namespace Api.Contracts.Books;

public sealed record UpdateBookRequest(
    string Title,
    string Isbn,
    DateTime PublishedOn,
    string? Description,
    string? ModifiedBy,
    bool State
);