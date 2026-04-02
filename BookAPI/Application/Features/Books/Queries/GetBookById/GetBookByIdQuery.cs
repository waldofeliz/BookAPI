using Application.Features.Books.Dtos;
using MediatR;

namespace Application.Features.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid Id) : IRequest<BookDto?>;