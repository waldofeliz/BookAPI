namespace Api.Contracts.Autores;

public sealed record UpdateAutorRequest(
    string Nombre,
    string Apellido,
    DateTime Cumpleanio,
    string? Biografia,
    string? Nacionalidad,
    DateTime? FechaFallecimiento,
    string? FotoUrl,
    string? SitioWeb,
    bool Estado
);
