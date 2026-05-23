using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using MediatR;

namespace Application.Features.Libros.Queries.GetLibroPorId;

public sealed class GetLibroPorIdHandler : IRequestHandler<GetLibroPorIdQuery, LibroDto?>
{
    private readonly ILibroRepository _repo;
    
    public GetLibroPorIdHandler(ILibroRepository repo) => _repo = repo;

    public async Task<LibroDto?> Handle(GetLibroPorIdQuery request, CancellationToken ct)
    {
        var libro = await _repo.GetByIdAsync(request.Id, ct);
        if (libro is null) return null;

        return new LibroDto(libro.Id, libro.Titulo, libro.Isbn, libro.PublicadoEn, libro.Descripcion);
    }
}