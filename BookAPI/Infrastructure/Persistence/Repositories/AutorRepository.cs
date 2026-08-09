using Application.Abstractions.Persistence;
using Application.Features.Autores.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Infrastructure.Persistence.Repositories;

public sealed class AutorRepository : IAutorRepository
{
    private readonly BookDbContext _db;

    public AutorRepository(BookDbContext db) => _db = db;

    public Task<Autor?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Autores.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByNombreYApellidoAsync(string nombre, string apellido, Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Autores.Where(b => b.Nombre.ToUpper().Equals(nombre.ToUpper())
                                       && b.Apellido.ToUpper().Equals(apellido.ToUpper()));
        if (excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }

    public async Task<bool> AllExistAsync(IEnumerable<Guid> ids, CancellationToken ct)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0) return true;
        var count = await _db.Autores.CountAsync(a => idList.Contains(a.Id), ct);
        return count == idList.Count;
    }

    public async Task<PagedResult<AutorDto>> ListAsync(int page, int pageSize, string? search, CancellationToken ct)
    {
        var query = _db.Autores.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(b => b.Nombre.Contains(s) || b.Apellido.Contains(s));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(b => b.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new AutorDto(b.Id, b.Nombre, b.Apellido, b.Cumpleanio, b.Biografia, b.Nacionalidad, b.FechaFallecimiento, b.FotoUrl, b.SitioWeb))
            .ToListAsync(ct);

        return new PagedResult<AutorDto>
        {
            Items = items,
            Meta = new PageMeta(page, pageSize, totalCount)
        };
    }

    public Task AddAsync(Autor autor, CancellationToken ct)
        => _db.Autores.AddAsync(autor, ct).AsTask();

    public void Update(Autor autor) => _db.Autores.Update(autor);

    public void Remove(Autor autor) => _db.Autores.Remove(autor);
}
