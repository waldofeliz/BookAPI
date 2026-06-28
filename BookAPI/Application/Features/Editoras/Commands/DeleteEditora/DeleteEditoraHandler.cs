using Application.Abstractions.Persistence;
using MediatR;

namespace Application.Features.Editoras.Commands.DeleteEditora;

public sealed class DeleteEditoraHandler : IRequestHandler<DeleteEditoraCommand, Unit>
{
    private readonly IEditoraRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteEditoraHandler(IEditoraRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteEditoraCommand request, CancellationToken ct)
    {
        var editora = await _repo.GetByIdAsync(request.Id, ct)
                      ?? throw new KeyNotFoundException("Editora no encontrada");

        _repo.Remove(editora);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
