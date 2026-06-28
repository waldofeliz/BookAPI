using Application.Features.Editoras.Dtos;
using MediatR;

namespace Application.Features.Editoras.Commands.CreateEditora;

public sealed record CreateEditoraCommand(
    string Nombre,
    string? Descripcion,
    string? Direccion,
    string? Pais,
    string? Website,
    string? Telefono,
    string CreadoPor
) : IRequest<EditoraDto>;
