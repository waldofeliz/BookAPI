using Domain.Entities;

namespace Application.Abstractions.Persistence;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByIsbnAsync(string isbn, Guid? excludeId, CancellationToken ct);
    
    Task AddAsync(Book book, CancellationToken ct);
    void Update(Book book);
    void Remove(Book book);

    IQueryable<Book> Query();
}