using Application.Abstractions.Persistence;
using Application.Features.Editoras.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Editoras.Commands.CreateEditora;

public sealed class CreateEditoraHandler : IRequestHandler<CreateEditoraCommand, EditoraDto>
{
    private readonly IEditoraRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateEditoraHandler(IEditoraRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<EditoraDto> Handle(CreateEditoraCommand request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByNombreAsync(request.Nombre, excludeId: null, ct);
        if (exists) throw new InvalidOperationException("Existe una editora con el mismo nombre");

        var editora = new Editora(
            request.Nombre,
            request.Descripcion,
            request.Direccion,
            request.Pais,
            request.Website,
            request.Telefono,
            request.CreadoPor);

        await _repo.AddAsync(editora, ct);
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
