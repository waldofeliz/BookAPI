using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Libros.Commands.CreateLibro;

public sealed class CreateLibroHandler : IRequestHandler<CreateLibroCommand, LibroDto>
{
    private readonly ILibroRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateLibroHandler(ILibroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<LibroDto> Handle(CreateLibroCommand request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByIsbnAsync(request.Isbn, excludeId: null, ct);
        if (exists) throw new InvalidOperationException("Existe un libro con el mismo ISBN");

        var libro = new Libro(request.Titulo, request.Isbn, request.PublicadoEn, request.Descripcion, request.CreadoPor);

        await _repo.AddAsync(libro, ct);
        await _uow.SaveChangesAsync(ct);
        
        return new LibroDto(libro.Id, libro.Titulo, libro.Isbn, libro.PublicadoEn, libro.Descripcion);
    }
}