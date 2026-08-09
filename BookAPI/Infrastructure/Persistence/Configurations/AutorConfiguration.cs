using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class AutorConfiguration: IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.ToTable("Autores");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nombre)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Apellido)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Cumpleanio)
            .IsRequired();

        builder.Property(x => x.Nacionalidad);

        builder.Property(x => x.Biografia);

        builder.Property(x => x.FechaFallecimiento);

        builder.Property(x => x.FotoUrl);

        builder.Property(x => x.SitioWeb);

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
    }
}
