using MediatR;

namespace Application.Features.Editoras.Commands.DeleteEditora;

public sealed record DeleteEditoraCommand(Guid Id) : IRequest<Unit>;
