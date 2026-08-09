using Application.Abstractions.Security;

namespace Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddBookApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.CanReadCatalog, policy =>
                policy.RequireRole(AppRoles.Admin, AppRoles.Editor, AppRoles.Reader));

            options.AddPolicy(AuthorizationPolicies.CanManageCatalog, policy =>
                policy.RequireRole(AppRoles.Admin, AppRoles.Editor));
        });

        return services;
    }
}
