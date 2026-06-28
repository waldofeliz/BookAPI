using Domain.Entities;

namespace Application.Features.Libros.Dtos;

internal static class LibroDtoMapper
{
    public static LibroDto ToDto(Libro libro) =>
        new(
            libro.Id,
            libro.Titulo,
            libro.Isbn,
            libro.PublicadoEn,
            libro.Descripcion,
            libro.CoverImageUrl,
            libro.Lenguaje,
            libro.Paginas,
            libro.Edicion,
            libro.SubTitulo,
            libro.EditoraId,
            libro.Editora?.Nombre,
            libro.LibroAutores
                .OrderBy(la => la.Orden)
                .Select(la => new AutorResumenDto(
                    la.Autor.Id,
                    la.Autor.Nombre,
                    la.Autor.Apellido,
                    la.Orden))
                .ToList());
}
