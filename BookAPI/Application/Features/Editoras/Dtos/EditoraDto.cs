namespace Application.Features.Editoras.Dtos;

public sealed record EditoraDto(
    Guid Id,
    string Nombre,
    string? Descripcion,
    string? Direccion,
    string? Pais,
    string? Website,
    string? Telefono,
    bool Estado
);