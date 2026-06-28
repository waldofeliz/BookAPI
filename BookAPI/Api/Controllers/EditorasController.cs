using Api.Contracts.Editoras;
using Application.Features.Editoras.Commands.CreateEditora;
using Application.Features.Editoras.Commands.DeleteEditora;
using Application.Features.Editoras.Commands.UpdateEditora;
using Application.Features.Editoras.Queries.GetEditoraPorId;
using Application.Features.Editoras.Queries.ListEditoras;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class EditorasController : ControllerBase
{
    private readonly IMediator _mediator;

    public EditorasController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEditoraPorIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListEditorasQuery(page, pageSize, search), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEditoraRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateEditoraCommand(
            request.Nombre,
            request.Descripcion,
            request.Direccion,
            request.Pais,
            request.Website,
            request.Telefono,
            request.CreadoPor), ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateEditoraRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateEditoraCommand(
            id,
            request.Nombre,
            request.Descripcion,
            request.Direccion,
            request.Pais,
            request.Website,
            request.Telefono,
            request.Estado,
            request.ModificadoPor), ct);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteEditoraCommand(id), ct);
        return NoContent();
    }
}
