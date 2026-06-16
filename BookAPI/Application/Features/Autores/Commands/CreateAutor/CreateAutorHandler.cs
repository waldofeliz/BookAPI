using Application.Abstractions.Persistence;
using Application.Features.Autores.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Autores.Commands.CreateAutor;

public sealed class CreateAutorHandler : IRequestHandler<CreateAutorCommand, AutorDto>
{
    private readonly IAutorRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateAutorHandler(IAutorRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }
    
    public async Task<AutorDto> Handle(CreateAutorCommand request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByNombreYApellidoAsync(request.Nombre, request.Apellido, excludeId: null, ct);
        if (exists) throw new InvalidOperationException("Existe un autor con el mismo nombre y apellido");
        
        var autor = new Autor(request.Nombre, request.Apellido, request.Biografia, request.Cumpleanio, request.CreadoPor);
        
        await _repo.AddAsync(autor, ct);
        await _uow.SaveChangesAsync(ct);

        return new AutorDto(autor.Id, autor.Nombre, autor.Apellido, autor.Cumpleanio, autor.Biografia);
    }
}