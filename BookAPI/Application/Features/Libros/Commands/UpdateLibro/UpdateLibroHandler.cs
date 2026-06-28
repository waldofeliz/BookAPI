using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using MediatR;

namespace Application.Features.Libros.Commands.UpdateLibro;

public sealed class UpdateLibroHandler : IRequestHandler<UpdateLibroCommand, LibroDto>
{
    private readonly ILibroRepository _repo;
    private readonly IEditoraRepository _editoraRepo;
    private readonly IAutorRepository _autorRepo;
    private readonly IUnitOfWork _uow;

    public UpdateLibroHandler(
        ILibroRepository repo,
        IEditoraRepository editoraRepo,
        IAutorRepository autorRepo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _editoraRepo = editoraRepo;
        _autorRepo = autorRepo;
        _uow = uow;
    }

    public async Task<LibroDto> Handle(UpdateLibroCommand request, CancellationToken ct)
    {
        var libro = await _repo.GetByIdWithDetailsAsync(request.Id, ct)
                   ?? throw new KeyNotFoundException("Libro no encontrado");

        var isExists = await _repo.ExistsByIsbnAsync(request.Isbn, excludeId: request.Id, ct);
        if (isExists) throw new InvalidOperationException("Ya existe un libro con el mismo ISBN.");

        await LibroRelationValidator.ValidateAsync(
            request.EditoraId,
            request.AutorIds,
            _editoraRepo,
            _autorRepo,
            ct);

        libro.Update(
            request.Titulo,
            request.Isbn,
            request.PublicadoEn,
            request.Descripcion,
            request.ModificadoPor,
            request.Estado,
            request.Lenguaje,
            request.Paginas,
            request.Edicion,
            request.CoverImageUrl,
            request.SubTitulo,
            request.EditoraId);

        libro.SyncAutores(request.AutorIds ?? []);

        _repo.Update(libro);
        await _uow.SaveChangesAsync(ct);

        var updated = await _repo.GetByIdWithDetailsAsync(libro.Id, ct)
                      ?? throw new InvalidOperationException("No se pudo recuperar el libro actualizado.");

        return LibroDtoMapper.ToDto(updated);
    }
}
