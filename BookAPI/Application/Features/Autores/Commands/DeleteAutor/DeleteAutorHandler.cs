using Application.Abstractions.Persistence;
using MediatR;

namespace Application.Features.Autores.Commands.DeleteAutor;

public sealed class DeleteAutorHandler : IRequestHandler<DeleteAutorCommand, Unit>
{
    private readonly IAutorRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteAutorHandler(IAutorRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }
    
    public async Task<Unit> Handle(DeleteAutorCommand request, CancellationToken ct)
    {
        var autor = await _repo.GetByIdAsync(request.Id, ct)
                    ?? throw new KeyNotFoundException("Autor no encontrado");
        
        _repo.Remove(autor);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}