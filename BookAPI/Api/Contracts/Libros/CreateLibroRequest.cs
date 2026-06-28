namespace Api.Contracts.Books;

public sealed record CreateLibroRequest
(
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    string? SubTitulo,
    string? CoverImageUrl,
    string? Edicion,
    int Paginas,
    string? Lenguaje,
    Guid? EditoraId,
    IReadOnlyList<Guid>? AutorIds
);
