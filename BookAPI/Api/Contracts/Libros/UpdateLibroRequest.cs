namespace Api.Contracts.Books;

public sealed record UpdateLibroRequest(
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    bool Estado,
    string? SubTitulo,
    string? CoverImageUrl,
    string? Edicion,
    int Paginas,
    string? Lenguaje,
    Guid? EditoraId,
    IReadOnlyList<Guid>? AutorIds
);
