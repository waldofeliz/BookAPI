using Application.Features.Libros.Dtos;
using Domain.Entities;
using Shared.Results;

namespace Application.Abstractions.Persistence;

public interface ILibroRepository
{
    Task<Libro?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Libro?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByIsbnAsync(string isbn, Guid? excludeId, CancellationToken ct);
    Task<PagedResult<LibroDto>> ListAsync(int page, int pageSize, string? search, CancellationToken ct);

    Task AddAsync(Libro libro, CancellationToken ct);
    void Update(Libro libro);
    void Remove(Libro libro);
}
