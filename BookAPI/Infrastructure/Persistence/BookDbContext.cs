using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class BookDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookDbContext).Assembly);
    }
}
