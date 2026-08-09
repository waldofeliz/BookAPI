using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Abstractions.Security;
using Testcontainers.MsSql;

namespace IntegrationTests;

[TestFixture]
public sealed class LibrosIntegrationTests
{
    private MsSqlContainer _sqlContainer = null!;
    private BookApiFactory _factory = null!;
    private HttpClient _client = null!;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _sqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        await _sqlContainer.StartAsync();

        _factory = new BookApiFactory(_sqlContainer.GetConnectionString());
        await _factory.MigrateDatabaseAsync();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
        await _sqlContainer.DisposeAsync();
    }

    [Test]
    public async Task CreateLibro_WithEditoraAndAutores_ReturnsRelations()
    {
        var token = await RegisterAndLoginAsync($"user_{Guid.NewGuid():N}@test.com", AppRoles.Editor);

        var editoraId = await CreateEditoraAsync(token, "Penguin Books");
        var autor1Id = await CreateAutorAsync(token, "Robert", "Martin");
        var autor2Id = await CreateAutorAsync(token, "Martin", "Fowler");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Libros");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            titulo = "Clean Architecture",
            isbn = $"978{Guid.NewGuid():N}"[..13],
            publicadoEn = new DateTime(2017, 9, 13),
            descripcion = "Arquitectura de software",
            paginas = 432,
            lenguaje = "en",
            editoraId,
            autorIds = new[] { autor1Id, autor2Id }
        });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));

        var libro = await response.Content.ReadFromJsonAsync<LibroResponse>(JsonOptions);
        Assert.That(libro, Is.Not.Null);
        Assert.That(libro!.EditoraId, Is.EqualTo(editoraId));
        Assert.That(libro.EditoraNombre, Is.EqualTo("Penguin Books"));
        Assert.That(libro.Autores, Has.Count.EqualTo(2));
        Assert.That(libro.Autores.Select(a => a.Id), Is.EquivalentTo(new[] { autor1Id, autor2Id }));
    }

    private async Task<string> RegisterAndLoginAsync(string email, string? role = null)
    {
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var registerResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            email,
            password
        });
        registerResponse.EnsureSuccessStatusCode();

        if (role is not null && !string.Equals(role, AppRoles.Reader, StringComparison.Ordinal))
            await AuthTestHelper.AssignRoleAsync(_factory, email, role);

        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        loginResponse.EnsureSuccessStatusCode();

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth!.AccessToken;
    }

    private async Task<Guid> CreateEditoraAsync(string token, string nombre)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Editoras");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { nombre });

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var editora = await response.Content.ReadFromJsonAsync<EntityResponse>(JsonOptions);
        return editora!.Id;
    }

    private async Task<Guid> CreateAutorAsync(string token, string nombre, string apellido)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Autores");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            nombre,
            apellido,
            cumpleanio = new DateTime(1970, 1, 1)
        });

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var autor = await response.Content.ReadFromJsonAsync<EntityResponse>(JsonOptions);
        return autor!.Id;
    }

    private sealed record AuthResponse(string AccessToken);
    private sealed record EntityResponse(Guid Id);
    private sealed record LibroResponse(
        Guid Id,
        Guid? EditoraId,
        string? EditoraNombre,
        IReadOnlyList<AutorResponse> Autores);
    private sealed record AutorResponse(Guid Id, string Nombre, string Apellido, int Orden);
}
