using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Abstractions.Security;
using Testcontainers.MsSql;

namespace IntegrationTests;

[TestFixture]
public sealed class AutoresIntegrationTests
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
    public async Task CrudAutor_FlujoCompleto_RespetaContratoHttp()
    {
        var token = await RegisterAndLoginAsync($"crud_{Guid.NewGuid():N}@test.com", AppRoles.Editor);

        // Create
        var cumpleanio = new DateTime(1980, 5, 15);
        using var createRequest = AuthorizedJsonPost("/api/v1/Autores", token, new
        {
            nombre = "Gabriel",
            apellido = "García Márquez",
            cumpleanio,
            biografia = "Escritor colombiano, premio Nobel de Literatura.",
            nacionalidad = "Colombia"
        });

        var createResponse = await _client.SendAsync(createRequest);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var created = await createResponse.Content.ReadFromJsonAsync<AutorResponse>(JsonOptions);
        Assert.That(created, Is.Not.Null);
        Assert.That(created!.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(created.Nombre, Is.EqualTo("Gabriel"));
        Assert.That(created.Apellido, Is.EqualTo("García Márquez"));
        Assert.That(created.Cumpleanio.Date, Is.EqualTo(cumpleanio.Date));
        Assert.That(created.Biografia, Is.EqualTo("Escritor colombiano, premio Nobel de Literatura."));
        Assert.That(created.Nacionalidad, Is.EqualTo("Colombia"));

        // GetById
        var getResponse = await _client.SendAsync(AuthorizedGet($"/api/v1/Autores/{created.Id}", token));
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var fetched = await getResponse.Content.ReadFromJsonAsync<AutorResponse>(JsonOptions);
        Assert.That(fetched!.Id, Is.EqualTo(created.Id));
        Assert.That(fetched.Nombre, Is.EqualTo("Gabriel"));

        // List
        var listResponse = await _client.SendAsync(AuthorizedGet("/api/v1/Autores?page=1&pageSize=10&search=García", token));
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var list = await listResponse.Content.ReadFromJsonAsync<PagedAutoresResponse>(JsonOptions);
        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Items, Has.Some.Matches<AutorResponse>(a => a.Id == created.Id));
        Assert.That(list.Meta.TotalCount, Is.GreaterThanOrEqualTo(1));

        // Update
        var updatedCumpleanio = new DateTime(1980, 5, 16);
        using var updateRequest = AuthorizedJsonPut($"/api/v1/Autores/{created.Id}", token, new
        {
            nombre = "Gabriel",
            apellido = "García Márquez",
            cumpleanio = updatedCumpleanio,
            biografia = "Biografía actualizada.",
            nacionalidad = "Colombia",
            estado = false
        });

        var updateResponse = await _client.SendAsync(updateRequest);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var updated = await updateResponse.Content.ReadFromJsonAsync<AutorResponse>(JsonOptions);
        Assert.That(updated!.Biografia, Is.EqualTo("Biografía actualizada."));
        Assert.That(updated.Cumpleanio.Date, Is.EqualTo(updatedCumpleanio.Date));

        // Delete
        using var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/Autores/{created.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var deleteResponse = await _client.SendAsync(deleteRequest);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        // GetById after delete
        var getAfterDelete = await _client.SendAsync(AuthorizedGet($"/api/v1/Autores/{created.Id}", token));
        Assert.That(getAfterDelete.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task CreateAutor_SinAutenticacion_Retorna401()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/Autores", new
        {
            nombre = "Anónimo",
            apellido = "Sin Token",
            cumpleanio = new DateTime(1990, 1, 1)
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateAutor_ConNombreDuplicado_Retorna409()
    {
        var token = await RegisterAndLoginAsync($"dup_{Guid.NewGuid():N}@test.com", AppRoles.Editor);
        var payload = new
        {
            nombre = "Isabel",
            apellido = "Allende",
            cumpleanio = new DateTime(1942, 8, 2),
            biografia = (string?)null,
            nacionalidad = "Chile"
        };

        using var firstRequest = AuthorizedJsonPost("/api/v1/Autores", token, payload);
        var firstResponse = await _client.SendAsync(firstRequest);
        Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        using var duplicateRequest = AuthorizedJsonPost("/api/v1/Autores", token, payload);
        var duplicateResponse = await _client.SendAsync(duplicateRequest);
        Assert.That(duplicateResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task CreateAutor_ConDatosInvalidos_Retorna400()
    {
        var token = await RegisterAndLoginAsync($"invalid_{Guid.NewGuid():N}@test.com", AppRoles.Editor);

        using var request = AuthorizedJsonPost("/api/v1/Autores", token, new
        {
            nombre = "",
            apellido = "",
            cumpleanio = default(DateTime)
        });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetAutorPorId_CuandoNoExiste_Retorna404()
    {
        var token = await RegisterAndLoginAsync($"notfound_{Guid.NewGuid():N}@test.com");

        var response = await _client.SendAsync(AuthorizedGet($"/api/v1/Autores/{Guid.NewGuid()}", token));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateAutor_CuandoNoExiste_Retorna404()
    {
        var token = await RegisterAndLoginAsync($"upd404_{Guid.NewGuid():N}@test.com", AppRoles.Editor);

        using var request = AuthorizedJsonPut($"/api/v1/Autores/{Guid.NewGuid()}", token, new
        {
            nombre = "Inexistente",
            apellido = "Autor",
            cumpleanio = new DateTime(1970, 1, 1),
            biografia = (string?)null,
            nacionalidad = (string?)null,
            estado = true
        });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DeleteAutor_CuandoNoExiste_Retorna404()
    {
        var token = await RegisterAndLoginAsync($"del404_{Guid.NewGuid():N}@test.com", AppRoles.Editor);

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/Autores/{Guid.NewGuid()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task<string> RegisterAndLoginAsync(string email, string? role = null)
    {
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var registerResponse = await _client.PostAsJsonAsync("/api/v1/auth/register", new { email, password });
        registerResponse.EnsureSuccessStatusCode();

        if (role is not null && !string.Equals(role, AppRoles.Reader, StringComparison.Ordinal))
            await AuthTestHelper.AssignRoleAsync(_factory, email, role);

        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        loginResponse.EnsureSuccessStatusCode();

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return auth!.AccessToken;
    }

    private static HttpRequestMessage AuthorizedGet(string url, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private static HttpRequestMessage AuthorizedJsonPost(string url, string token, object body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(body);
        return request;
    }

    private static HttpRequestMessage AuthorizedJsonPut(string url, string token, object body)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(body);
        return request;
    }

    private sealed record AuthResponse(string AccessToken);

    private sealed record AutorResponse(
        Guid Id,
        string Nombre,
        string Apellido,
        DateTime Cumpleanio,
        string? Biografia,
        string? Nacionalidad);

    private sealed record PagedAutoresResponse(
        IReadOnlyList<AutorResponse> Items,
        PageMetaResponse Meta);

    private sealed record PageMetaResponse(int Page, int PageSize, int TotalCount);
}
