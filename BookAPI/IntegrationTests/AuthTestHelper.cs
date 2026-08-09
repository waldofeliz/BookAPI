using System.Net.Http.Json;
using System.Text.Json;
using Application.Abstractions.Security;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public static class AuthTestHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task<string> RegisterAndLoginAsync(
        HttpClient client,
        BookApiFactory factory,
        string email,
        string? role = null)
    {
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var registerResponse = await client.PostAsJsonAsync("/api/v1/auth/register", new { email, password });
        registerResponse.EnsureSuccessStatusCode();

        if (role is not null && !string.Equals(role, AppRoles.Editor, StringComparison.Ordinal))
            await AssignRoleAsync(factory, email, role);

        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        loginResponse.EnsureSuccessStatusCode();

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth!.AccessToken;
    }

    public static async Task<string> LoginAsync(HttpClient client, string email, string password)
    {
        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        loginResponse.EnsureSuccessStatusCode();

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth!.AccessToken;
    }

    public static async Task AssignRoleAsync(BookApiFactory factory, string email, string role)
    {
        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (!await roleManager.RoleExistsAsync(role))
        {
            var createRole = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            if (!createRole.Succeeded)
            {
                throw new InvalidOperationException(
                    $"No se pudo crear el rol {role}: {string.Join(", ", createRole.Errors.Select(e => e.Description))}");
            }
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            throw new InvalidOperationException($"Usuario de prueba no encontrado: {email}");

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
            await userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"No se pudo asignar el rol {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    private sealed record AuthResponse(string AccessToken);
}
