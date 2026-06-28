using Application.Abstractions.Persistence;
using Application.Features.Autores.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Features.Autores.Queries.ListAutores;

public sealed class ListAutoresHandler : IRequestHandler<ListAutoresQuery, PagedResult<AutorDto>>
{
    private readonly IAutorRepository _repo;

    public ListAutoresHandler(IAutorRepository repo) => _repo = repo;

    public async Task<PagedResult<AutorDto>> Handle(ListAutoresQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(b => b.Nombre.Contains(s) || b.Apellido.Contains(s));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(b => b.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new AutorDto(b.Id, b.Nombre, b.Apellido, b.Cumpleanio, b.Biografia, b.Nacionalidad))
            .ToListAsync(ct);

        return new PagedResult<AutorDto>
        {
            Items = items,
            Meta = new PageMeta(page, pageSize, totalCount)
        };
    }
}