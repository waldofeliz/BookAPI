using Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity;

public sealed class AdminBootstrapSeeder : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly AdminBootstrapOptions _options;
    private readonly ILogger<AdminBootstrapSeeder> _logger;

    public AdminBootstrapSeeder(
        IServiceProvider services,
        IOptions<AdminBootstrapOptions> options,
        ILogger<AdminBootstrapSeeder> logger)
    {
        _services = services;
        _options = options.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.BootstrapEmails.Length == 0)
            return;

        try
        {
            using var scope = _services.CreateScope();
            var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userRoles = scope.ServiceProvider.GetRequiredService<IUserRoleService>();

            foreach (var email in _options.BootstrapEmails.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                var normalized = email.Trim();
                var user = await users.FindByEmailAsync(normalized);
                if (user is null)
                {
                    _logger.LogWarning("Bootstrap admin omitido: usuario {Email} no registrado.", normalized);
                    continue;
                }

                await userRoles.PromoteToAdminAsync(normalized, cancellationToken);
                _logger.LogInformation("Usuario {Email} promovido a Admin por bootstrap.", normalized);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Se omitió el bootstrap de administradores durante el arranque. Se reintentará cuando el esquema esté disponible.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
