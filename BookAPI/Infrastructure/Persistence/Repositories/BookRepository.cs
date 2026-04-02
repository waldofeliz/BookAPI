using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly BookDbContext _db;
    
    public BookRepository(BookDbContext db) => _db = db;
    
    public IQueryable<Book> Query() => _db.Books;

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Books.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<bool> ExistsByIsbnAsync(string isbn, Guid? excludeId, CancellationToken ct)
    {
        var q = _db.Books.Where(b => b.Isbn == isbn);
        if(excludeId.HasValue) q = q.Where(b => b.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }
    
    public Task AddAsync(Book book, CancellationToken ct)
    => _db.Books.AddAsync(book, ct).AsTask();
    
    public void Update(Book book) => _db.Books.Update(book);
    
    public void Remove(Book book) => _db.Books.Remove(book);
}