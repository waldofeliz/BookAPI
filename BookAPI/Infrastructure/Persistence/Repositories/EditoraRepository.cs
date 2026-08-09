using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Infrastructure.Persistence.Repositories;

public sealed class EditoraRepository : IEditoraRepository
{
    private readonly BookDbContext _db;

    public EditoraRepository(BookDbContext db) => _db = db;

    public Task<Editora?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Editoras.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByNombreAsync(string nombre, Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Editoras.Where(b => b.Nombre.ToUpper().Equals(nombre.ToUpper()));
        if (excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }

    public async Task<PagedResult<EditoraDto>> ListAsync(int page, int pageSize, string? search, CancellationToken ct)
    {
        var query = _db.Editoras.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
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

    public Task AddAsync(Editora editora, CancellationToken ct)
        => _db.Editoras.AddAsync(editora, ct).AsTask();

    public void Update(Editora editora) => _db.Editoras.Update(editora);

    public void Remove(Editora editora) => _db.Editoras.Remove(editora);
}
