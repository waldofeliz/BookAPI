using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public sealed class BookApiFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;
    private readonly int? _authPermitLimit;

    public BookApiFactory(string connectionString, int? authPermitLimit = null)
    {
        _connectionString = connectionString;
        _authPermitLimit = authPermitLimit;
    }

    public async Task MigrateDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();
        await db.Database.MigrateAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _connectionString);
        builder.UseSetting("Jwt:SecretKey", "IntegrationTestsSecretKey_Min32Chars!");
        builder.UseSetting("Jwt:Issuer", "BookAPI");
        builder.UseSetting("Jwt:Audience", "BookAPI.Clients");
        builder.UseSetting("Jwt:RequireHttpsMetadata", "false");
        builder.UseSetting("Cors:AllowedOrigins:0", "http://localhost");
        builder.UseSetting("RateLimiting:Auth:PermitLimit",
            (_authPermitLimit ?? 10_000).ToString());
        builder.UseSetting("RateLimiting:Auth:WindowSeconds", "60");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BookDbContext>));
            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<BookDbContext>(options =>
                options.UseSqlServer(_connectionString));
        });
    }
}
