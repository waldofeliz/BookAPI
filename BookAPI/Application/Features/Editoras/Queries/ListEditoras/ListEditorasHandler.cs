using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Features.Editoras.Queries.ListEditoras;

public sealed class ListEditorasHandler : IRequestHandler<ListEditorasQuery, PagedResult<EditoraDto>>
{
    private readonly IEditoraRepository _repo;

    public ListEditorasHandler(IEditoraRepository repo) => _repo = repo;

    public async Task<PagedResult<EditoraDto>> Handle(ListEditorasQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(e =>
                e.Nombre.Contains(s) ||
                (e.Pais != null && e.Pais.Contains(s)) ||
                (e.Descripcion != null && e.Descripcion.Contains(s)));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(e => e.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EditoraDto(
                e.Id,
                e.Nombre,
                e.Descripcion,
                e.Direccion,
                e.Pais,
                e.Website,
                e.Telefono,
                e.Estado))
            .ToListAsync(ct);

        return new PagedResult<EditoraDto>
        {
            Items = items,
            Meta = new PageMeta(page, pageSize, totalCount)
        };
    }
}