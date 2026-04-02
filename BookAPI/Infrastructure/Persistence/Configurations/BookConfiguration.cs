using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Isbn)
            .HasMaxLength(17)
            .IsRequired();

        builder.HasIndex(x => x.Isbn)
            .IsUnique();
        
        builder.Property(x => x.PublishedOn)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000);
    }
}