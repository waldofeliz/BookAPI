using Application.Abstractions.Persistence;
using MediatR;

namespace Application.Features.Libros.Commands.DeleteLibro;

public sealed class DeleteLibroHandler : IRequestHandler<DeleteLibroCommand, Unit>
{
    private readonly ILibroRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteLibroHandler(ILibroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteLibroCommand request, CancellationToken ct)
    {
        var libro = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Libro no encontrado");
        
        _repo.Remove(libro);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}