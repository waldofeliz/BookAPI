using Application.Features.Libros.Dtos;
using MediatR;

namespace Application.Features.Libros.Commands.UpdateLibro;

public sealed record UpdateLibroCommand(
    Guid Id,
    string Titulo,
    string Isbn,
    DateTime PublicadoEn,
    string? Descripcion,
    string? ModificadoPor,
    bool Estado
) : IRequest<LibroDto>;