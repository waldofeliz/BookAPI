using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using Application.Features.Libros.Queries.ListLibros;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Libros.Queries.ListLibros;

public sealed class ListLibrosHandler : IRequestHandler<ListLibrosQuery, IReadOnlyList<LibroDto>>
{
    private readonly ILibroRepository _repo;

    public ListLibrosHandler(ILibroRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<LibroDto>> Handle(ListLibrosQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

        var query = _repo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(b => b.Titulo.Contains(s) || b.Isbn.Contains(s));
        }

        var items = await query
            .OrderByDescending(b => b.PublicadoEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new LibroDto(b.Id, b.Titulo, b.Isbn, b.PublicadoEn, b.Descripcion, b.CoverImageUrl, b.Lenguaje, b.Paginas, b.Edicion, b.SubTitulo))
            .ToListAsync(ct);
        return items;
    }
}