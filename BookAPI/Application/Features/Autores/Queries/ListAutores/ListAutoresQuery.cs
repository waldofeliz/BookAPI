using Application.Features.Autores.Dtos;
using MediatR;

namespace Application.Features.Autores.Queries.ListAutores;

public sealed record ListAutoresQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
 ): IRequest<IReadOnlyList<AutorDto>>;