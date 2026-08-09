using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using MediatR;
using Shared.Results;

namespace Application.Features.Libros.Queries.ListLibros;

public sealed class ListLibrosHandler : IRequestHandler<ListLibrosQuery, PagedResult<LibroDto>>
{
    private readonly ILibroRepository _repo;

    public ListLibrosHandler(ILibroRepository repo) => _repo = repo;

    public Task<PagedResult<LibroDto>> Handle(ListLibrosQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        return _repo.ListAsync(page, pageSize, request.Search, ct);
    }
}
