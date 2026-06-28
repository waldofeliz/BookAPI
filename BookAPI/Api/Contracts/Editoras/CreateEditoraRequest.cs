namespace Api.Contracts.Editoras;

public sealed record CreateEditoraRequest(
    string Nombre,
    string? Descripcion,
    string? Direccion,
    string? Pais,
    string? Website,
    string? Telefono
);