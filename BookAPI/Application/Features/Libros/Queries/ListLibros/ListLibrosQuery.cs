using Application.Features.Libros.Dtos;
using MediatR;
using Shared.Results;

namespace Application.Features.Libros.Queries.ListLibros;

public sealed record ListLibrosQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<PagedResult<LibroDto>>;