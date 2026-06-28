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
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(x => x.SubTitulo)
            .HasMaxLength(250);

        builder.Property(x => x.Isbn)
            .HasMaxLength(17)
            .IsRequired();

        builder.HasIndex(x => x.Isbn)
            .IsUnique();
        
        builder.Property(x => x.PublicadoEn);
        
       builder.HasIndex(x => x.Paginas)
            .IsUnique(false);

        builder.Property(x => x.Lenguaje)
            .HasMaxLength(50);
        
        builder.Property(x => x.Edicion)
            .HasMaxLength(50);
        
        builder.Property(x => x.CoverImageUrl)
            .HasMaxLength(500);
        
        builder.Property(x => x.Estado)
            .IsRequired();
        
        builder.Property(x => x.CreadoPor)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.CreadoEn)
            .IsRequired();
        
        builder.Property(x => x.ModificadoPor)
            .HasMaxLength(100);
        
        builder.Property(x => x.ModificadoEn);
        
        builder.Property(x => x.Version)
            .IsConcurrencyToken()
            .IsRequired();

        builder.Property(x => x.EditoraId);

        builder.HasOne(x => x.Editora)
            .WithMany()
            .HasForeignKey(x => x.EditoraId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.EditoraId);
    }
}