using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Features.Libros.Queries.ListLibros;

public sealed class ListLibrosHandler : IRequestHandler<ListLibrosQuery, PagedResult<LibroDto>>
{
    private readonly ILibroRepository _repo;

    public ListLibrosHandler(ILibroRepository repo) => _repo = repo;

    public async Task<PagedResult<LibroDto>> Handle(ListLibrosQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(b =>
                b.Titulo.Contains(s) ||
                b.Isbn.Contains(s) ||
                (b.Editora != null && b.Editora.Nombre.Contains(s)));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(b => b.PublicadoEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new LibroDto(
                b.Id,
                b.Titulo,
                b.Isbn,
                b.PublicadoEn,
                b.Descripcion,
                b.CoverImageUrl,
                b.Lenguaje,
                b.Paginas,
                b.Edicion,
                b.SubTitulo,
                b.EditoraId,
                b.Editora != null ? b.Editora.Nombre : null,
                b.LibroAutores
                    .OrderBy(la => la.Orden)
                    .Select(la => new AutorResumenDto(
                        la.Autor.Id,
                        la.Autor.Nombre,
                        la.Autor.Apellido,
                        la.Orden))
                    .ToList()))
            .ToListAsync(ct);

        return new PagedResult<LibroDto>
        {
            Items = items,
            Meta = new PageMeta(page, pageSize, totalCount)
        };
    }
}
