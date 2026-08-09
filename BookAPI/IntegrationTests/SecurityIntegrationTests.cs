using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Abstractions.Security;
using Testcontainers.MsSql;

namespace IntegrationTests;

[TestFixture]
public sealed class SecurityIntegrationTests
{
    private MsSqlContainer _sqlContainer = null!;
    private BookApiFactory _factory = null!;
    private HttpClient _client = null!;

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
        _client?.Dispose();
        if (_factory is not null)
            await _factory.DisposeAsync();
        if (_sqlContainer is not null)
            await _sqlContainer.DisposeAsync();
    }

    [Test]
    public async Task CreateAutor_ConRolReader_Retorna403()
    {
        var email = $"reader_{Guid.NewGuid():N}@test.com";
        var token = await AuthTestHelper.RegisterAndLoginAsync(_client, _factory, email, AppRoles.Reader);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Autores");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            nombre = "Solo",
            apellido = "Lectura",
            cumpleanio = new DateTime(1990, 1, 1)
        });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task ListAutores_ConRolReader_Retorna200()
    {
        var email = $"readerlist_{Guid.NewGuid():N}@test.com";
        var token = await AuthTestHelper.RegisterAndLoginAsync(_client, _factory, email, AppRoles.Reader);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/Autores?page=1&pageSize=5");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Login_ExcedeLimiteDeTasa_Retorna429()
    {
        await using var rateLimitedFactory = new BookApiFactory(
            _sqlContainer.GetConnectionString(),
            authPermitLimit: 2);

        await rateLimitedFactory.MigrateDatabaseAsync();
        using var client = rateLimitedFactory.CreateClient();

        var email = $"ratelimit_{Guid.NewGuid():N}@test.com";

        var first = await client.PostAsJsonAsync("/api/v1/auth/login",
            new { email, password = "wrong-password" });
        var second = await client.PostAsJsonAsync("/api/v1/auth/login",
            new { email, password = "wrong-password" });
        var third = await client.PostAsJsonAsync("/api/v1/auth/login",
            new { email, password = "wrong-password" });

        Assert.That(first.StatusCode, Is.Not.EqualTo(HttpStatusCode.TooManyRequests));
        Assert.That(second.StatusCode, Is.Not.EqualTo(HttpStatusCode.TooManyRequests));
        Assert.That(third.StatusCode, Is.EqualTo(HttpStatusCode.TooManyRequests));
    }

    [Test]
    public async Task PromoteUser_ConRolAdmin_Retorna204()
    {
        var adminEmail = $"admin_{Guid.NewGuid():N}@test.com";
        var targetEmail = $"promote_{Guid.NewGuid():N}@test.com";
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var adminToken = await AuthTestHelper.RegisterAndLoginAsync(_client, _factory, adminEmail, AppRoles.Admin);

        var registerTarget = await _client.PostAsJsonAsync("/api/v1/auth/register",
            new { email = targetEmail, password });
        registerTarget.EnsureSuccessStatusCode();

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Admin/users/promote");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        request.Content = JsonContent.Create(new { email = targetEmail });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task PromoteUser_SinRolAdmin_Retorna403()
    {
        var editorEmail = $"editor_{Guid.NewGuid():N}@test.com";
        var targetEmail = $"target403_{Guid.NewGuid():N}@test.com";
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var editorToken = await AuthTestHelper.RegisterAndLoginAsync(_client, _factory, editorEmail, AppRoles.Editor);

        await _client.PostAsJsonAsync("/api/v1/auth/register", new { email = targetEmail, password });

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Admin/users/promote");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", editorToken);
        request.Content = JsonContent.Create(new { email = targetEmail });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task AssignRole_ConAdminAsignaReader_UsuarioQuedaSoloLectura()
    {
        var adminEmail = $"admin_assign_{Guid.NewGuid():N}@test.com";
        var targetEmail = $"reader_assign_{Guid.NewGuid():N}@test.com";
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var adminToken = await AuthTestHelper.RegisterAndLoginAsync(_client, _factory, adminEmail, AppRoles.Admin);
        await _client.PostAsJsonAsync("/api/v1/auth/register", new { email = targetEmail, password });

        using (var assignRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Admin/users/assign-role"))
        {
            assignRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            assignRequest.Content = JsonContent.Create(new { email = targetEmail, role = AppRoles.Reader });

            var assignResponse = await _client.SendAsync(assignRequest);
            Assert.That(assignResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        var readerToken = await AuthTestHelper.LoginAsync(_client, targetEmail, password);

        using var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Autores");
        createRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", readerToken);
        createRequest.Content = JsonContent.Create(new
        {
            nombre = "Sin",
            apellido = "Escritura",
            cumpleanio = new DateTime(1990, 1, 1)
        });

        var createResponse = await _client.SendAsync(createRequest);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task AssignRole_SinRolAdmin_Retorna403()
    {
        var editorEmail = $"editor_assign403_{Guid.NewGuid():N}@test.com";
        var targetEmail = $"target_assign403_{Guid.NewGuid():N}@test.com";
        var password = Environment.GetEnvironmentVariable("BOOKAPI_TEST_PASSWORD")
                       ?? $"TestPass_{Guid.NewGuid():N}!Aa1";

        var editorToken = await AuthTestHelper.RegisterAndLoginAsync(_client, _factory, editorEmail, AppRoles.Editor);
        await _client.PostAsJsonAsync("/api/v1/auth/register", new { email = targetEmail, password });

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/Admin/users/assign-role");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", editorToken);
        request.Content = JsonContent.Create(new { email = targetEmail, role = AppRoles.Reader });

        var response = await _client.SendAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}
