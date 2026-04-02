using Application.Abstractions.Persistence;
using Application.Features.Books.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetBookById;

public sealed class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IBookRepository _repo;
    
    public GetBookByIdHandler(IBookRepository repo) => _repo = repo;

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken ct)
    {
        var book = await _repo.GetByIdAsync(request.Id, ct);
        if (book is null) return null;

        return new BookDto(book.Id, book.Title, book.Isbn, book.PublishedOn, book.Description);
    }
}