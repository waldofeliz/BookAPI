using Application.Features.Editoras.Dtos;
using MediatR;

namespace Application.Features.Editoras.Commands.UpdateEditora;

public sealed record UpdateEditoraCommand(
    Guid Id,
    string Nombre,
    string? Descripcion,
    string? Direccion,
    string? Pais,
    string? Website,
    string? Telefono,
    bool Estado,
    string? ModificadoPor
) : IRequest<EditoraDto>;
