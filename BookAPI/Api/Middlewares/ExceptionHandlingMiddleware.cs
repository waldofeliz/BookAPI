using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        IHostEnvironment environment,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status401Unauthorized, "No autorizado", ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Validation error", ex.Message);
        }
        catch (ValidationException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Validation error", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Key not found", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status409Conflict, "Conflict", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            var detail = _environment.IsDevelopment()
                ? ex.Message
                : "Ocurrió un error interno. Consulte los logs del servidor.";

            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Server error", detail);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = title,
            Status = status,
            Detail = detail
        });
    }
}
