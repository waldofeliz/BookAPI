namespace Application.Abstractions.Security;

public interface IUserRoleService
{
    Task PromoteToAdminAsync(string email, CancellationToken ct);

    Task AssignCatalogRoleAsync(string email, string role, CancellationToken ct);
}
