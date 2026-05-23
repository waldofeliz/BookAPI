using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class LibroConfiguration : IEntityTypeConfiguration<Libro>
{
    public void Configure(EntityTypeBuilder<Libro> builder)
    {
        builder.ToTable("Libros");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Isbn)
            .HasMaxLength(17)
            .IsRequired();

        builder.HasIndex(x => x.Isbn)
            .IsUnique();
        
        builder.Property(x => x.PublicadoEn)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(2000);
    }
}