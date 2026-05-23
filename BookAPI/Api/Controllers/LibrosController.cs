using Api.Contracts.Books;
using Application.Features.Libros.Commands.CreateLibro;
using Application.Features.Libros.Commands.UpdateLibro;
using Application.Features.Libros.Queries.GetLibroPorId;
using Application.Features.Libros.Commands.DeleteLibro;
using Application.Features.Libros.Queries.ListLibros;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class LibrosController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public LibrosController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLibroRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateLibroCommand(
            request.Titulo,
            request.Isbn,
            request.PublicadoEn,
            request.Descripcion,
            request.CreadoPor
            ), ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLibroPorIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListLibrosQuery(page, pageSize, search), ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateLibroRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateLibroCommand(
            id,
            request.Titulo,
            request.Isbn,
            request.PublicadoEn,
            request.Descripcion,
            request.ModificadoPor,
            request.Estado
        ), ct);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteLibroCommand(id), ct);
        return NoContent();
    }
}