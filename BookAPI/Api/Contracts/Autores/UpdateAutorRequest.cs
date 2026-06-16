namespace Api.Contracts.Autores;

public sealed record UpdateAutorRequest(
    string Nombre,
    string Apellido,
    DateTime Cumpleanio,
    string? Biografia,
    string ModificadoPor,
    bool Estado
    );