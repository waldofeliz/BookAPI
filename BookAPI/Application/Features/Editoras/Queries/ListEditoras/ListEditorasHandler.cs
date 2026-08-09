using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using MediatR;
using Shared.Results;

namespace Application.Features.Editoras.Queries.ListEditoras;

public sealed class ListEditorasHandler : IRequestHandler<ListEditorasQuery, PagedResult<EditoraDto>>
{
    private readonly IEditoraRepository _repo;

    public ListEditorasHandler(IEditoraRepository repo) => _repo = repo;

    public Task<PagedResult<EditoraDto>> Handle(ListEditorasQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        return _repo.ListAsync(page, pageSize, request.Search, ct);
    }
}
