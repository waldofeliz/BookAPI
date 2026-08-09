namespace Application.Abstractions.Security;

public static class AuthorizationPolicies
{
    public const string CanReadCatalog = nameof(CanReadCatalog);
    public const string CanManageCatalog = nameof(CanManageCatalog);
}
