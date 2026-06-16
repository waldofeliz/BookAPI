using Application.Features.Autores.Dtos;
using MediatR;

namespace Application.Features.Autores.Commands.UpdateAutor;

public sealed record UpdateAutorCommand(
    Guid Id,
    string Nombre,
    string Apellido,
    DateTime Cumpleanio,
    string? Biografia,
    string? ModificadoPor,
    bool Estado
): IRequest<AutorDto>;