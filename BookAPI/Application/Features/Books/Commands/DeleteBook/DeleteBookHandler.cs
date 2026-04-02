using Application.Abstractions.Persistence;
using MediatR;

namespace Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private readonly IBookRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteBookHandler(IBookRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken ct)
    {
        var book = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Book not found.");
        
        _repo.Remove(book);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}