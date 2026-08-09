using Application.Features.Editoras.Dtos;
using Domain.Entities;
using Shared.Results;

namespace Application.Abstractions.Persistence;

public interface IEditoraRepository
{
    Task<Editora?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByNombreAsync(string nombre, Guid? excludeId, CancellationToken ct);
    Task<PagedResult<EditoraDto>> ListAsync(int page, int pageSize, string? search, CancellationToken ct);

    Task AddAsync(Editora editora, CancellationToken ct);
    void Update(Editora editora);
    void Remove(Editora editora);
}
