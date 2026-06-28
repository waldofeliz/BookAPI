using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class EditoraRepository: IEditoraRepository
{
    private readonly BookDbContext _db;
    
    public EditoraRepository(BookDbContext db) => _db = db;
    
    public IQueryable<Editora> Query() => _db.Editoras;

    public Task<Editora?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Editoras.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByNombreAsync(string nombre,Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Editoras.Where(b => b.Nombre.ToUpper().Equals(nombre.ToUpper()));
        if(excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }

    public Task AddAsync(Editora editora, CancellationToken ct)
        => _db.Editoras.AddAsync(editora, ct).AsTask();
    
    public void Update(Editora editora) => _db.Editoras.Update(editora);
    
    public void Remove(Editora editora) => _db.Editoras.Remove(editora);
}