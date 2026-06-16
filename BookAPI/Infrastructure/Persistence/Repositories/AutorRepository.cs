using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class AutorRepository: IAutorRepository
{
    private readonly BookDbContext _db;
    
    public AutorRepository(BookDbContext db) => _db = db;
    
    public IQueryable<Autor> Query() => _db.Autores;

    public Task<Autor?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Autores.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByNombreYApellidoAsync(string nombre, string apellido,Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Autores.Where(b => b.Nombre.ToUpper().Equals(nombre.ToUpper()) 
                                       && b.Apellido.ToUpper().Equals(apellido.ToUpper()));
        if(excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }

    public Task AddAsync(Autor autor, CancellationToken ct)
        => _db.Autores.AddAsync(autor, ct).AsTask();
    
    public void Update(Autor autor) => _db.Autores.Update(autor);
    
    public void Remove(Autor autor) => _db.Autores.Remove(autor);
}