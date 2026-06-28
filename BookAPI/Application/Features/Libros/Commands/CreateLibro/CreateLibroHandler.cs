using Application.Abstractions.Persistence;
using Application.Features.Libros.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Libros.Commands.CreateLibro;

public sealed class CreateLibroHandler : IRequestHandler<CreateLibroCommand, LibroDto>
{
    private readonly ILibroRepository _repo;
    private readonly IEditoraRepository _editoraRepo;
    private readonly IAutorRepository _autorRepo;
    private readonly IUnitOfWork _uow;

    public CreateLibroHandler(
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

    public async Task<LibroDto> Handle(CreateLibroCommand request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByIsbnAsync(request.Isbn, excludeId: null, ct);
        if (exists) throw new InvalidOperationException("Existe un libro con el mismo ISBN");

        await LibroRelationValidator.ValidateAsync(
            request.EditoraId,
            request.AutorIds,
            _editoraRepo,
            _autorRepo,
            ct);

        var libro = new Libro(
            request.Titulo,
            request.Isbn,
            request.PublicadoEn,
            request.Descripcion,
            request.CreadoPor,
            request.Lenguaje,
            request.Paginas,
            request.Edicion,
            request.CoverImageUrl,
            request.SubTitulo,
            request.EditoraId);

        if (request.AutorIds is { Count: > 0 })
            libro.SyncAutores(request.AutorIds);

        await _repo.AddAsync(libro, ct);
        await _uow.SaveChangesAsync(ct);

        var created = await _repo.GetByIdWithDetailsAsync(libro.Id, ct)
                      ?? throw new InvalidOperationException("No se pudo recuperar el libro creado.");

        return LibroDtoMapper.ToDto(created);
    }
}
