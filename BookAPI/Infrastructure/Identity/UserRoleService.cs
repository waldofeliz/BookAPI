using Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public sealed class UserRoleService : IUserRoleService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly RoleManager<IdentityRole<Guid>> _roles;

    public UserRoleService(UserManager<ApplicationUser> users, RoleManager<IdentityRole<Guid>> roles)
    {
        _users = users;
        _roles = roles;
    }

    public async Task PromoteToAdminAsync(string email, CancellationToken ct)
    {
        var user = await FindUserOrThrowAsync(email);

        await EnsureRoleExistsAsync(AppRoles.Admin);

        if (await _users.IsInRoleAsync(user, AppRoles.Admin))
            return;

        await AddRoleOrThrowAsync(user, AppRoles.Admin, "No se pudo promover al usuario a Admin.");
    }

    public async Task AssignCatalogRoleAsync(string email, string role, CancellationToken ct)
    {
        if (!AppRoles.AssignableCatalogRoles.Contains(role))
            throw new ArgumentException($"El rol '{role}' no se puede asignar mediante este endpoint.");

        var user = await FindUserOrThrowAsync(email);

        await EnsureRoleExistsAsync(role);

        var currentCatalogRoles = await _users.GetRolesAsync(user);
        var rolesToRemove = currentCatalogRoles
            .Where(r => AppRoles.AssignableCatalogRoles.Contains(r))
            .ToList();

        if (rolesToRemove.Count > 0)
        {
            var removeResult = await _users.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                var msg = string.Join(" ", removeResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    string.IsNullOrWhiteSpace(msg) ? "No se pudieron actualizar los roles del usuario." : msg);
            }
        }

        if (await _users.IsInRoleAsync(user, role))
            return;

        await AddRoleOrThrowAsync(user, role, $"No se pudo asignar el rol {role} al usuario.");
    }

    private async Task<ApplicationUser> FindUserOrThrowAsync(string email)
    {
        var user = await _users.FindByEmailAsync(email.Trim());
        if (user is null)
            throw new KeyNotFoundException($"No se encontró un usuario con el email '{email}'.");

        return user;
    }

    private async Task AddRoleOrThrowAsync(ApplicationUser user, string role, string fallbackMessage)
    {
        var result = await _users.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            var msg = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(msg) ? fallbackMessage : msg);
        }
    }

    private async Task EnsureRoleExistsAsync(string role)
    {
        if (await _roles.RoleExistsAsync(role))
            return;

        var result = await _roles.CreateAsync(new IdentityRole<Guid>(role));
        if (!result.Succeeded)
        {
            var msg = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(msg)
                ? $"No se pudo crear el rol {role}."
                : msg);
        }
    }
}
