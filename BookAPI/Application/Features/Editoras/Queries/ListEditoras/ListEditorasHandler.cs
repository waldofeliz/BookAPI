using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Editoras.Queries.ListEditoras;

public sealed class ListEditorasHandler : IRequestHandler<ListEditorasQuery, IReadOnlyList<EditoraDto>>
{
    private readonly IEditoraRepository _repo;

    public ListEditorasHandler(IEditoraRepository repo) => _repo = repo;
    
    public async Task<IReadOnlyList<EditoraDto>> Handle(ListEditorasQuery request, CancellationToken ct)
    {
        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(e =>
                e.Nombre.Contains(s) ||
                (e.Pais != null && e.Pais.Contains(s)) ||
                (e.Descripcion != null && e.Descripcion.Contains(s)));
        }

        var items = await query
            .OrderBy(e => e.Nombre)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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

        return items;
    }
}