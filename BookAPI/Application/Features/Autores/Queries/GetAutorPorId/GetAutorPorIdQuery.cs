using Application.Features.Autores.Dtos;
using MediatR;

namespace Application.Features.Autores.Queries.GetAutorPorId;

public sealed record GetAutorPorIdQuery(Guid Id) : IRequest<AutorDto?>;