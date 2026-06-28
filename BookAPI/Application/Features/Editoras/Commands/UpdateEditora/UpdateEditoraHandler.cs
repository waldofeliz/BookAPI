using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using MediatR;

namespace Application.Features.Editoras.Commands.UpdateEditora;

public sealed class UpdateEditoraHandler : IRequestHandler<UpdateEditoraCommand, EditoraDto>
{
    private readonly IEditoraRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateEditoraHandler(IEditoraRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<EditoraDto> Handle(UpdateEditoraCommand request, CancellationToken ct)
    {
        var editora = await _repo.GetByIdAsync(request.Id, ct)
                      ?? throw new KeyNotFoundException("Editora no encontrada");

        var exists = await _repo.ExistsByNombreAsync(request.Nombre, excludeId: request.Id, ct);
        if (exists) throw new InvalidOperationException("Ya existe una editora con el mismo nombre");

        editora.Update(
            request.Nombre,
            request.Descripcion,
            request.Direccion,
            request.Pais,
            request.Website,
            request.Telefono,
            request.ModificadoPor,
            request.Estado);

        _repo.Update(editora);
        await _uow.SaveChangesAsync(ct);

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
