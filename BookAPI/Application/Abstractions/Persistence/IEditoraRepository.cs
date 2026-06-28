using Domain.Entities;

namespace Application.Abstractions.Persistence;

public interface IEditoraRepository
{
    Task<Editora?> GetByIdAsync(Guid id, CancellationToken ct);
    
    Task<bool> ExistsByNombreAsync(string nombre, Guid? excludeId, CancellationToken ct);
    
    Task AddAsync(Editora editora, CancellationToken ct);
    void Update(Editora editora);
    void Remove(Editora editora);

    IQueryable<Editora> Query();
}