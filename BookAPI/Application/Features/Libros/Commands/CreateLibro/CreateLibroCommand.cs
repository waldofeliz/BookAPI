using Application.Features.Libros.Dtos;
using MediatR;

namespace Application.Features.Libros.Commands.CreateLibro;

public sealed record CreateLibroCommand(
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    string CreadoPor,
    string? CoverImageUrl,
    string? Lenguaje,
    int Paginas,
    string? Edicion,
    string? SubTitulo,
    Guid? EditoraId,
    IReadOnlyList<Guid>? AutorIds
) : IRequest<LibroDto>;
