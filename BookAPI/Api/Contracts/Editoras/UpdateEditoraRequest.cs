namespace Api.Contracts.Editoras;

public sealed record UpdateEditoraRequest(
    string Nombre,
    string? Descripcion,
    string? Direccion,
    string? Pais,
    string? Website,
    string? Telefono,
    bool Estado,
    string ModificadoPor
    );