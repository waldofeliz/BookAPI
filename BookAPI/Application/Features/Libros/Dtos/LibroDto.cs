namespace Application.Features.Libros.Dtos;

public sealed record LibroDto
(
    Guid Id,
    string Titulo,
    string Isbn,
    DateTime? PublicadoEn,
    string? Descripcion,
    string? coverImageUrl,
    string? Lenguaje,
    int Paginas,
    string? Edicion,
    string? SubTitulo
 );