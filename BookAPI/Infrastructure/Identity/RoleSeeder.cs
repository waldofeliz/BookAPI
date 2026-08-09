using Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity;

public sealed class RoleSeeder : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<RoleSeeder> _logger;

    public RoleSeeder(IServiceProvider services, ILogger<RoleSeeder> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var roleName in AppRoles.All)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                    continue;

                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (result.Succeeded)
                    _logger.LogInformation("Rol {Role} creado.", roleName);
                else
                    _logger.LogWarning("No se pudo crear el rol {Role}: {Errors}",
                        roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Se omitió el seeding de roles durante el arranque. Se reintentará cuando el esquema esté disponible.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
