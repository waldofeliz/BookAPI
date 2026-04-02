using Application.Abstractions.Persistence;
using Application.Features.Books.Dtos;
using MediatR;

namespace Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateBookHandler(IBookRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken ct)
    {
        var book = await _repo.GetByIdAsync(request.Id, ct)
                   ?? throw new KeyNotFoundException("Book not found.");

        var isExists = await _repo.ExistsByIsbnAsync(request.Isbn, excludeId: request.Id, ct);
        if(isExists) throw new InvalidOperationException("A book with the same ISBN already exists.");
        
        book.Update(request.Title, request.Isbn, request.PublishedOn, request.Description);
        
        _repo.Update(book);
        await _uow.SaveChangesAsync(ct);
        
        return new BookDto(book.Id, book.Title, book.Isbn, book.PublishedOn, book.Description);
    }
}