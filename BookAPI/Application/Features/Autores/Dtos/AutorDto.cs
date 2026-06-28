namespace Application.Features.Autores.Dtos;

public sealed record AutorDto
(
    Guid Id,
    string Nombre,
    string Apellido,
    DateTime Cumpleanio,
    string? Biografia,
    string? Nacionalidad
);