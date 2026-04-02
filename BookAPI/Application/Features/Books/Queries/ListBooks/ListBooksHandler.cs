using Application.Abstractions.Persistence;
using Application.Features.Books.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Books.Queries.ListBooks;

public sealed class ListBooksHandler : IRequestHandler<ListBooksQuery, IReadOnlyList<BookDto>>
{
    private readonly IBookRepository _repo;
    
    public ListBooksHandler(IBookRepository repo) =>  _repo = repo;

    public async Task<IReadOnlyList<BookDto>> Handle(ListBooksQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(b => b.Title.Contains(s) || b.Isbn.Contains(s));
        }

        var items = await query
            .OrderByDescending(b => b.PublishedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookDto(b.Id, b.Title, b.Isbn, b.PublishedOn, b.Description))
            .ToListAsync(ct);
        return items;
    }
}