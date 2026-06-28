namespace Application.Features.Libros.Dtos;

public sealed record LibroDto(
    Guid Id,
    string Titulo,
    string Isbn,
    DateTime? PublicadoEn,
    string? Descripcion,
    string? CoverImageUrl,
    string? Lenguaje,
    int Paginas,
    string? Edicion,
    string? SubTitulo,
    Guid? EditoraId,
    string? EditoraNombre,
    IReadOnlyList<AutorResumenDto> Autores
);
