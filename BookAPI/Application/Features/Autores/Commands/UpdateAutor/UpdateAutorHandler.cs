using Application.Abstractions.Persistence;
using Application.Features.Autores.Dtos;
using MediatR;

namespace Application.Features.Autores.Commands.UpdateAutor;

public sealed class UpdateAutorHandler : IRequestHandler<UpdateAutorCommand, AutorDto>
{
    private readonly IAutorRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAutorHandler(IAutorRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }
    
    public async Task<AutorDto> Handle(UpdateAutorCommand request, CancellationToken ct)
    {
        var autor = await _repo.GetByIdAsync(request.Id, ct)
                    ?? throw new KeyNotFoundException("Autor no encontrado");

        var isExists = await _repo.ExistsByNombreYApellidoAsync(request.Nombre, request.Apellido, excludeId: request.Id, ct);
        if (isExists) throw new InvalidOperationException("Ya existe un autor con el mismo nombre y apellido");
        
        autor.Update(request.Nombre, request.Apellido, request.Biografia, request.Cumpleanio, request.ModificadoPor, request.Estado, request.Nacionalidad);
        
        _repo.Update(autor);

        await _uow.SaveChangesAsync(ct);
        
        return new AutorDto(autor.Id, autor.Nombre, autor.Apellido, autor.Cumpleanio, autor.Biografia, autor.Nacionalidad);
    }
}