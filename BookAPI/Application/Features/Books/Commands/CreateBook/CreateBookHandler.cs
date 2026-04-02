using Application.Abstractions.Persistence;
using Application.Features.Books.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IBookRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateBookHandler(IBookRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken ct)
    {
        var exists = await _repo.ExistsByIsbnAsync(request.Isbn, excludeId: null, ct);
        if (exists) throw new InvalidOperationException("A book with the same ISBN already exists.");

        var book = new Book(request.Title, request.Isbn, request.PublishedOn, request.Description);

        await _repo.AddAsync(book, ct);
        await _uow.SaveChangesAsync(ct);
        
        return new BookDto(book.Id, book.Title, book.Isbn, book.PublishedOn, book.Description);
    }
}