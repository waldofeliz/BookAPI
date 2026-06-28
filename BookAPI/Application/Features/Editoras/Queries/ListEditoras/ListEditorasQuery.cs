using Application.Features.Editoras.Dtos;
using MediatR;
using Shared.Results;

namespace Application.Features.Editoras.Queries.ListEditoras;

public sealed record ListEditorasQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<PagedResult<EditoraDto>>;