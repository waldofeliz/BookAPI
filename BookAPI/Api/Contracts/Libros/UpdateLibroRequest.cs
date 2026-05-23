namespace Api.Contracts.Books;

public sealed record UpdateLibroRequest(
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    string? ModificadoPor,
    bool Estado
);