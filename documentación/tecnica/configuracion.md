# Configuración

## Archivos de configuración

| Archivo | Ubicación | Propósito |
|---------|-----------|-----------|
| `appsettings.json` | `BookAPI/Api/` | Configuración base (sin secretos) |
| `appsettings.Development.json` | `BookAPI/Api/` | Overrides para desarrollo local |
| `launchSettings.json` | `BookAPI/Api/Properties/` | Perfiles de ejecución y puertos |
| `Directory.Build.props` | `BookAPI/` | Configuración global .NET 9 |

## Secciones de configuración

### ConnectionStrings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

En **Development** (`appsettings.Development.json`):
```
Server=localhost,1433;Database=BookAPI;User Id=sa;Password=***;TrustServerCertificate=True
```

### Jwt

```json
{
  "Jwt": {
    "Issuer": "BookAPI",
    "Audience": "BookAPI.Clients",
    "SecretKey": "",
    "AccessTokenMinutes": 15,
    "RefreshTokenDays": 7,
    "RequireHttpsMetadata": true
  }
}
```

| Clave | Descripción | Requerido |
|-------|-------------|-----------|
| `Issuer` | Emisor del token JWT | Sí |
| `Audience` | Audiencia esperada | Sí |
| `SecretKey` | Clave simétrica HMAC (mín. 32 chars) | Sí (falla startup si falta) |
| `AccessTokenMinutes` | Duración del access token | No (default: 15) |
| `RefreshTokenDays` | Duración del refresh token | No (default: 7) |
| `RequireHttpsMetadata` | Validación HTTPS en metadata JWT | No (false en dev) |

### Cors

```json
{
  "Cors": {
    "AllowedOrigins": []
  }
}
```

En Development:
```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:5173"
    ]
  }
}
```

**Comportamiento:**
- Si `AllowedOrigins` tiene valores → solo esos orígenes
- Si está vacío y el entorno es Development → `AllowAnyOrigin`
- En Production sin orígenes configurados → CORS restrictivo

### Serilog

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    }
  }
}
```

## Variables de entorno

ASP.NET Core mapea variables con doble guión bajo (`__`):

| Variable | Equivalente appsettings |
|----------|------------------------|
| `ConnectionStrings__DefaultConnection` | Cadena SQL Server |
| `Jwt__SecretKey` | Clave JWT |
| `Jwt__Issuer` | Emisor |
| `Jwt__Audience` | Audiencia |
| `Jwt__AccessTokenMinutes` | Duración access token |
| `Jwt__RefreshTokenDays` | Duración refresh token |
| `Jwt__RequireHttpsMetadata` | HTTPS metadata |
| `Cors__AllowedOrigins__0` | Primer origen CORS |
| `Cors__AllowedOrigins__1` | Segundo origen CORS |
| `ASPNETCORE_ENVIRONMENT` | `Development` / `Production` |
| `ASPNETCORE_URLS` | URLs de escucha (`http://+:8080`) |

### Docker Compose

| Variable | Servicio | Default |
|----------|----------|---------|
| `MSSQL_SA_PASSWORD` | sqlserver | `Admin123!Secure` |
| `JWT_SECRET_KEY` | api | placeholder de 32+ chars |

## User Secrets (desarrollo local)

```bash
cd BookAPI/Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;..."
dotnet user-secrets set "Jwt:SecretKey" "TU_CLAVE_SEGURA_DE_AL_MENOS_32_CARACTERES"
```

## Puertos locales

| Perfil | URL |
|--------|-----|
| HTTP | `http://localhost:5143` |
| HTTPS | `https://localhost:7101` |
| Swagger | `/swagger` |
| Health | `/health` |

## Validaciones al arrancar

La aplicación **no inicia** si:

1. Falta `ConnectionStrings:DefaultConnection`
2. `Jwt:SecretKey` está vacío o tiene menos de 32 caracteres
