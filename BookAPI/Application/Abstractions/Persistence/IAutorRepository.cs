using Application.Features.Autores.Dtos;
using Domain.Entities;
using Shared.Results;

namespace Application.Abstractions.Persistence;

public interface IAutorRepository
{
    Task<Autor?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByNombreYApellidoAsync(string nombre, string apellido, Guid? excludeId, CancellationToken ct);
    Task<bool> AllExistAsync(IEnumerable<Guid> ids, CancellationToken ct);
    Task<PagedResult<AutorDto>> ListAsync(int page, int pageSize, string? search, CancellationToken ct);

    Task AddAsync(Autor autor, CancellationToken ct);
    void Update(Autor autor);
    void Remove(Autor autor);
}
