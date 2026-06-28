namespace Api.Contracts.Books;

public sealed record UpdateLibroRequest(
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    string? ModificadoPor,
    bool Estado,
    string? SubTitulo,
    string? CoverImageUrl,
    string? Edicion,
    int Paginas,
    string? Lenguaje
);