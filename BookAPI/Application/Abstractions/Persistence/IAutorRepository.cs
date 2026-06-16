using Domain.Entities;

namespace Application.Abstractions.Persistence;

public interface IAutorRepository
{
    Task<Autor?> GetByIdAsync(Guid id, CancellationToken ct);
    
    Task<bool> ExistsByNombreYApellidoAsync(string nombre, string apellido,Guid? excludeId, CancellationToken ct);
    
    Task AddAsync(Autor autor, CancellationToken ct);
    void Update(Autor autor);
    void Remove(Autor autor);

    IQueryable<Autor> Query();
}