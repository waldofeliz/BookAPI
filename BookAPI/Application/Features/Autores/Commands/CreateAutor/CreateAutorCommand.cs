using Application.Features.Autores.Dtos;
using MediatR;

namespace Application.Features.Autores.Commands.CreateAutor;

public sealed record CreateAutorCommand(
    string Nombre,
    string Apellido,
    DateTime Cumpleanio,
    string? Biografia,
    string CreadoPor,
    string? Nacionalidad
    ): IRequest<AutorDto>;