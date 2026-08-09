using Application.Abstractions.Persistence;
using Application.Features.Autores.Dtos;
using MediatR;
using Shared.Results;

namespace Application.Features.Autores.Queries.ListAutores;

public sealed class ListAutoresHandler : IRequestHandler<ListAutoresQuery, PagedResult<AutorDto>>
{
    private readonly IAutorRepository _repo;

    public ListAutoresHandler(IAutorRepository repo) => _repo = repo;

    public Task<PagedResult<AutorDto>> Handle(ListAutoresQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        return _repo.ListAsync(page, pageSize, request.Search, ct);
    }
}
