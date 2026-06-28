using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using MediatR;

namespace Application.Features.Libros.Commands.UpdateLibro;

public sealed class UpdateLibroHandler : IRequestHandler<UpdateLibroCommand, LibroDto>
{
    private readonly ILibroRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateLibroHandler(ILibroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<LibroDto> Handle(UpdateLibroCommand request, CancellationToken ct)
    {
        var libro = await _repo.GetByIdAsync(request.Id, ct)
                   ?? throw new KeyNotFoundException("Libro no encontrado");

        var isExists = await _repo.ExistsByIsbnAsync(request.Isbn, excludeId: request.Id, ct);
        if (isExists) throw new InvalidOperationException("Ya existe un libro con el mismo ISBN.");

        libro.Update(request.Titulo, request.Isbn, request.PublicadoEn, request.Descripcion, request.ModificadoPor, request.Estado, request.Lenguaje, request.Paginas, request.Edicion, request.CoverImageUrl);

        _repo.Update(libro);
        await _uow.SaveChangesAsync(ct);

        return new LibroDto(libro.Id, libro.Titulo, libro.Isbn, libro.PublicadoEn, libro.Descripcion, libro.CoverImageUrl, libro.Lenguaje, libro.Paginas, libro.Edicion, libro.SubTitulo);
    }
}