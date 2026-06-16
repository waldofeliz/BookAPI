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
    }
}