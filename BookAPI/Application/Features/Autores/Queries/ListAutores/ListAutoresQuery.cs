using Application.Features.Autores.Dtos;
using MediatR;
using Shared.Results;

namespace Application.Features.Autores.Queries.ListAutores;

public sealed record ListAutoresQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<PagedResult<AutorDto>>;