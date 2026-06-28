using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using MediatR;

namespace Application.Features.Editoras.Queries.GetEditoraPorId;

public sealed class GetEditoraPorIdHandler : IRequestHandler<GetEditoraPorIdQuery, EditoraDto?>
{
    private readonly IEditoraRepository _repo;

    public GetEditoraPorIdHandler(IEditoraRepository repo) => _repo = repo;

    public async Task<EditoraDto?> Handle(GetEditoraPorIdQuery request, CancellationToken ct)
    {
        var editora = await _repo.GetByIdAsync(request.Id, ct);
        if (editora is null) return null;

        return new EditoraDto(
            editora.Id,
            editora.Nombre,
            editora.Descripcion,
            editora.Direccion,
            editora.Pais,
            editora.Website,
            editora.Telefono,
            editora.Estado);
    }
}