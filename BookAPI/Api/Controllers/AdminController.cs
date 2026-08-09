using Api.Contracts.Admin;
using Application.Abstractions.Security;
using Application.Features.Admin.Commands.AssignUserRole;
using Application.Features.Admin.Commands.PromoteUserToAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize(Roles = AppRoles.Admin)]
[Route("api/v1/[controller]")]
public sealed class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Promueve un usuario existente al rol Admin.
    /// </summary>
    [HttpPost("users/promote")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PromoteUser([FromBody] PromoteUserRequest request, CancellationToken ct)
    {
        await _mediator.Send(new PromoteUserToAdminCommand(request.Email), ct);
        return NoContent();
    }

    /// <summary>
    /// Asigna el rol de catálogo Editor o Reader a un usuario existente.
    /// </summary>
    [HttpPost("users/assign-role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole([FromBody] AssignUserRoleRequest request, CancellationToken ct)
    {
        await _mediator.Send(new AssignUserRoleCommand(request.Email, request.Role), ct);
        return NoContent();
    }
}
