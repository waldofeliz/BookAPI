using Api.Contracts.Books;
using Application.Features.Books.Commands.CreateBook;
using Application.Features.Books.Commands.DeleteBook;
using Application.Features.Books.Commands.UpdateBook;
using Application.Features.Books.Queries.GetBookById;
using Application.Features.Books.Queries.ListBooks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class BooksController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public BooksController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateBookCommand(
            request.Title,
            request.Isbn,
            request.PublishedOn,
            request.Description
            ), ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListBooksQuery(page, pageSize, search), ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateBookCommand(
            id,
            request.Title,
            request.Isbn,
            request.PublishedOn,
            request.Description
        ), ct);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBookCommand(id), ct);
        return NoContent();
    }
}