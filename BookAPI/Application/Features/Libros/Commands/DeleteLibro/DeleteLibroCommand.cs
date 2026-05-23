using MediatR;

namespace Application.Features.Libros.Commands.DeleteLibro;

public sealed record DeleteLibroCommand(Guid Id) : IRequest<Unit>;