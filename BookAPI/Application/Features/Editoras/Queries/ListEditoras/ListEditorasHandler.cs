using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Editoras.Queries.ListEditoras;

public sealed class ListEditorasHandler: IRequestHandler<ListEditorasQuery, IReadOnlyList<EditoraDto>>
{
    private readonly IEditoraRepository _repo;

    public ListEditorasHandler(IEditoraRepository repo) => _repo = repo;
    
    public async Task<IReadOnlyList<EditoraDto>> Handle(ListEditorasQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(b => b.Nombre.Contains(s));
        }

        var items = await query
            .OrderByDescending(b => b.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new EditoraDto(b.Nombre, b.Descripcion, b.Direccion, b.Pais, b.Website, b.Telefono))
            .ToListAsync(ct);
        
        return items;
    }
}