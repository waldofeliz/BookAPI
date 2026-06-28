using Application.Features.Editoras.Dtos;
using MediatR;

namespace Application.Features.Editoras.Queries.GetEditoraPorId;

public sealed record GetEditoraPorIdQuery(Guid Id) : IRequest<EditoraDto?>;