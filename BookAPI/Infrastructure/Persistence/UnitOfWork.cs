using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly BookDbContext _db;
    
    public UnitOfWork(BookDbContext db) =>  _db = db;
    
    public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}