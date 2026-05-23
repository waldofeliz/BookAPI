namespace Api.Contracts.Books;

public sealed record CreateLibroRequest
(
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    string CreadoPor
);