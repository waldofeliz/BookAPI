using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Infrastructure.Persistence.Repositories;

public sealed class LibroRepository : ILibroRepository
{
    private readonly BookDbContext _db;

    public LibroRepository(BookDbContext db) => _db = db;

    public Task<Libro?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Libros.FirstOrDefaultAsync(b => b.Id == id, ct);

    public Task<Libro?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct)
        => _db.Libros
            .Include(l => l.Editora)
            .Include(l => l.LibroAutores)
            .ThenInclude(la => la.Autor)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByIsbnAsync(string isbn, Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Libros.Where(b => b.Isbn == isbn);
        if (excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }

    public async Task<PagedResult<LibroDto>> ListAsync(int page, int pageSize, string? search, CancellationToken ct)
    {
        var query = _db.Libros.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(b =>
                b.Titulo.Contains(s) ||
                b.Isbn.Contains(s) ||
                (b.Editora != null && b.Editora.Nombre.Contains(s)));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(b => b.PublicadoEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new LibroDto(
                b.Id,
                b.Titulo,
                b.Isbn,
                b.PublicadoEn,
                b.Descripcion,
                b.CoverImageUrl,
                b.Lenguaje,
                b.Paginas,
                b.Edicion,
                b.SubTitulo,
                b.EditoraId,
                b.Editora != null ? b.Editora.Nombre : null,
                b.LibroAutores
                    .OrderBy(la => la.Orden)
                    .Select(la => new AutorResumenDto(
                        la.Autor.Id,
                        la.Autor.Nombre,
                        la.Autor.Apellido,
                        la.Orden))
                    .ToList()))
            .ToListAsync(ct);

        return new PagedResult<LibroDto>
        {
            Items = items,
            Meta = new PageMeta(page, pageSize, totalCount)
        };
    }

    public Task AddAsync(Libro libro, CancellationToken ct)
        => _db.Libros.AddAsync(libro, ct).AsTask();

    public void Update(Libro libro) => _db.Libros.Update(libro);

    public void Remove(Libro libro) => _db.Libros.Remove(libro);
}
