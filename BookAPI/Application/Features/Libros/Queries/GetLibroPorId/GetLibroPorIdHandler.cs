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
        var libro = await _repo.GetByIdWithDetailsAsync(request.Id, ct);
        return libro is null ? null : LibroDtoMapper.ToDto(libro);
    }
}
