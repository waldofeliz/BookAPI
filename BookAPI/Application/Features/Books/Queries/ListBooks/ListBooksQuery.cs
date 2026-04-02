using Application.Features.Books.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.ListBooks;

public sealed record ListBooksQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<IReadOnlyList<BookDto>>;