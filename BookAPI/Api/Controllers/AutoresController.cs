using Api.Contracts.Autores;
using Application.Abstractions.Security;
using Application.Features.Autores.Commands.CreateAutor;
using Application.Features.Autores.Commands.DeleteAutor;
using Application.Features.Autores.Commands.UpdateAutor;
using Application.Features.Autores.Queries.GetAutorPorId;
using Application.Features.Autores.Queries.ListAutores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public sealed class AutoresController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public AutoresController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAutorPorIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListAutoresQuery(page, pageSize, search), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAutorRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateAutorCommand(
            request.Nombre,
            request.Apellido,
            request.Cumpleanio,
            request.Biografia,
            _currentUser.GetUserName(),
            request.Nacionalidad), ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAutorRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateAutorCommand(
            id,
            request.Nombre,
            request.Apellido,
            request.Cumpleanio,
            request.Biografia,
            _currentUser.GetUserName(),
            request.Estado,
            request.Nacionalidad), ct);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAutorCommand(id), ct);
        return NoContent();
    }
}
