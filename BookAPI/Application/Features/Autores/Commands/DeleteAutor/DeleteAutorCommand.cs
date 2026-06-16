using MediatR;

namespace Application.Features.Autores.Commands.DeleteAutor;

public sealed record DeleteAutorCommand(Guid Id) : IRequest<Unit>;