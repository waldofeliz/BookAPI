using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class LibroRepository : ILibroRepository
{
    private readonly BookDbContext _db;
    
    public LibroRepository(BookDbContext db) => _db = db;
    
    public IQueryable<Libro> Query() => _db.Libros;

    public Task<Libro?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Libros.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByIsbnAsync(string isbn, Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Libros.Where(b => b.Isbn == isbn);
        if(excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }
    
    public Task AddAsync(Libro libro, CancellationToken ct)
    => _db.Libros.AddAsync(libro, ct).AsTask();
    
    public void Update(Libro libro) => _db.Libros.Update(libro);
    
    public void Remove(Libro libro) => _db.Libros.Remove(libro);
}