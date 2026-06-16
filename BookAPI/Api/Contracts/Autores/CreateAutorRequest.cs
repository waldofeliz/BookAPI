namespace Api.Contracts.Autores;

public sealed record CreateAutorRequest(
    string Nombre,
    string Apellido,
    DateTime Cumpleanio,
    string? Biografia,
    string CreadoPor
    );