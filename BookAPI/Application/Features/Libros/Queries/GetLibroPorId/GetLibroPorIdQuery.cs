using Application.Features.Libros.Dtos;
using MediatR;

namespace Application.Features.Libros.Queries.GetLibroPorId;

public sealed record GetLibroPorIdQuery(Guid Id) : IRequest<LibroDto?>;