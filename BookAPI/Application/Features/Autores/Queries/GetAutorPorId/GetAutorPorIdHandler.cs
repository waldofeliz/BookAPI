using Application.Abstractions.Persistence;
using Application.Features.Autores.Dtos;
using MediatR;

namespace Application.Features.Autores.Queries.GetAutorPorId;

public sealed class GetAutorPorIdHandler : IRequestHandler<GetAutorPorIdQuery, AutorDto>
{
    private readonly IAutorRepository _repo;

    public GetAutorPorIdHandler(IAutorRepository repo) => _repo = repo;

    public async Task<AutorDto?> Handle(GetAutorPorIdQuery request, CancellationToken cts)
    {
        var autor = await _repo.GetByIdAsync(request.Id, cts);
        if (autor is null) return null;

        return new AutorDto(
            autor.Id,
            autor.Nombre,
            autor.Apellido,
            autor.Cumpleanio,
            autor.Biografia,
            autor.Nacionalidad);
    }
}