using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class LibroAutorConfiguration : IEntityTypeConfiguration<LibroAutor>
{
    public void Configure(EntityTypeBuilder<LibroAutor> builder)
    {
        builder.ToTable("LibroAutores");

        builder.HasKey(x => new { x.LibroId, x.AutorId });

        builder.Property(x => x.Orden).IsRequired();

        builder.HasOne(x => x.Libro)
            .WithMany(x => x.LibroAutores)
            .HasForeignKey(x => x.LibroId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Autor)
            .WithMany(x => x.LibroAutores)
            .HasForeignKey(x => x.AutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.AutorId);
    }
}
