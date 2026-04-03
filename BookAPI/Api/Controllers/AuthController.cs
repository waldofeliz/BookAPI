using Api.Contracts.Auth;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshSession;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _mediator.Send(new RegisterCommand(request.Email, request.Password, ip), ct);
        return Ok(Map(result));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password, ip), ct);
        return Ok(Map(result));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _mediator.Send(new RefreshSessionCommand(request.RefreshToken, ip), ct);
        return Ok(Map(result));
    }

    private static AuthResponse Map(AuthTokensDto dto)
        => new()
        {
            AccessToken = dto.AccessToken,
            RefreshToken = dto.RefreshToken,
            AccessTokenExpiresAtUtc = dto.AccessTokenExpiresAtUtc
        };
}
