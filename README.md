# BookAPI

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

API REST para la gestión de libros, construida con **ASP.NET Core 9**, arquitectura en capas (Clean Architecture), **MediatR**, **Entity Framework Core**, **SQL Server**, autenticación **JWT** con **ASP.NET Core Identity** y **tokens de refresco** con rotación.

## Requisitos previos

| Herramienta | Versión recomendada |
|-------------|---------------------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 9.x |
| [SQL Server](https://www.microsoft.com/sql-server) | 2019+ (local, Docker o instancia en la nube) |
| Opcional: [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) | Global (`dotnet tool install -g dotnet-ef`) |

En **macOS** puedes usar SQL Server en Docker, Azure SQL Edge o una base remota; ajusta la cadena de conexión en consecuencia.

## Estructura de la solución

La solución está en la carpeta `BookAPI/` (archivo `BookAPI.sln`).

| Proyecto | Descripción |
|----------|-------------|
| **Api** | ASP.NET Core: controladores, contratos HTTP, Swagger, middleware |
| **Application** | Casos de uso: comandos/consultas MediatR, validación FluentValidation, DTOs |
| **Domain** | Entidades de dominio (por ejemplo `Book`, `RefreshToken`) |
| **Infrastructure** | EF Core, repositorios, Identity, emisión y validación JWT |
| **Shared** | Código compartido (si aplica) |
| **UnitTests** / **IntegrationTests** | Pruebas |

## Configuración

### Cadena de conexión

En `BookAPI/Api/appsettings.json` (o variables de entorno) define `ConnectionStrings:DefaultConnection`.

Ejemplo local (SQL Server en `localhost:1433`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=BookAPI;User Id=SA;Password=TU_PASSWORD;TrustServerCertificate=True"
}
```

**No subas contraseñas reales al repositorio.** En desarrollo usa [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
cd BookAPI/Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=..."
```

### JWT

Sección `Jwt` en `appsettings.json`:

| Clave | Descripción |
|-------|-------------|
| `Issuer` | Emisor del token |
| `Audience` | Audiencia esperada |
| `SecretKey` | Clave simétrica (**mínimo 32 caracteres**); en producción usar secretos seguros |
| `AccessTokenMinutes` | Vida del access token |
| `RefreshTokenDays` | Vida del refresh token |
| `RequireHttpsMetadata` | Metadatos OIDC solo por HTTPS (`false` suele usarse en desarrollo local) |

Sobrescribir con User Secrets o variables de entorno, por ejemplo:

```bash
dotnet user-secrets set "Jwt:SecretKey" "TU_CLAVE_LARGA_Y_ALEATORIA_MINIMO_32_CARACTERES"
```

Variables de entorno (ASP.NET Core las aplica automáticamente):

- `ConnectionStrings__DefaultConnection`
- `Jwt__SecretKey`, `Jwt__Issuer`, `Jwt__Audience`, etc.

## Base de datos y migraciones

Desde la carpeta que contiene `BookAPI.sln`:

```bash
cd BookAPI
```

Aplicar migraciones (crea/actualiza tablas: `Books`, Identity, `RefreshTokens`, etc.):

```bash
dotnet ef database update \
  --project Infrastructure/Infrastructure.csproj \
  --startup-project Api/Api.csproj
```

Crear una nueva migración tras cambiar el modelo:

```bash
dotnet ef migrations add NombreDeLaMigracion \
  --project Infrastructure/Infrastructure.csproj \
  --startup-project Api/Api.csproj
```

## Ejecutar en local

```bash
cd BookAPI
dotnet restore
dotnet build
cd Api
dotnet run
```

Por defecto (según `launchSettings.json`):

- HTTP: `http://localhost:5143`
- HTTPS: `https://localhost:7101`

Swagger UI: `http://localhost:5143/swagger` o `https://localhost:7101/swagger` (según perfil).

## API: autenticación y recursos

### Auth (público)

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/v1/auth/register` | Registro (email y contraseña) |
| POST | `/api/v1/auth/login` | Inicio de sesión |
| POST | `/api/v1/auth/refresh` | Renovar access token con refresh token |

Respuesta típica: `accessToken`, `refreshToken`, `accessTokenExpiresAtUtc`.

### Libros (requieren JWT)

Prefijo: `/api/v1/Books`

Incluye cabecera: `Authorization: Bearer <access_token>`.

En Swagger, usa **Authorize** e introduce el token tras hacer login o register.

## Pruebas

```bash
cd BookAPI
dotnet test
```

## Subir el proyecto (Git y despliegue)

### 1. Repositorio Git

```bash
git init
git add .
git commit -m "Initial commit"
git remote add origin <URL-de-tu-repositorio>
git branch -M main
git push -u origin main
```

Asegúrate de tener un `.gitignore` adecuado para .NET (por ejemplo desde [gitignore.io](https://www.toptal.com/developers/gitignore) con `VisualStudio`, `Rider`, `dotnet`) para **no** versionar `bin/`, `obj/`, `appsettings.Development.json` si contiene secretos, archivos de usuario locales, etc.

### 2. Checklist antes de producción

- [ ] `Jwt:SecretKey` única y larga; fuera del código (variables de entorno, secret manager del proveedor).
- [ ] `ConnectionStrings:DefaultConnection` apuntando a la base en la nube o servidor seguro.
- [ ] `RequireHttpsMetadata` acorde al entorno; HTTPS en el sitio público.
- [ ] Revisar CORS si el front consumirá otro origen (configurar en `Program.cs`).
- [ ] Ejecutar `dotnet ef database update` contra la base de destino o pipeline CI/CD.
- [ ] Publicar en **Release**: `dotnet publish -c Release -o ./publish`.

### 3. Publicar la aplicación

```bash
cd BookAPI/Api
dotnet publish -c Release -o ../../publish
```

El contenido de `publish/` es lo que suele desplegarse en IIS, Kestrel detrás de Nginx, contenedores, Azure App Service, AWS, etc.

### 4. Variables en el servidor o PaaS

Configura al menos:

- Cadena de conexión SQL.
- Claves JWT (`Jwt__*`).
- `ASPNETCORE_ENVIRONMENT=Production` (o el nombre que uses).

### 5. Base de datos en la nube

Crea la base en tu proveedor (Azure SQL, AWS RDS SQL Server, etc.), abre firewall/reglas para el origen de la API y usa la cadena de conexión en variables de entorno. Aplica migraciones desde tu máquina con red permitida o desde el pipeline de CI/CD.

## Documentación interactiva

Con la API en marcha, **Swagger UI** documenta los endpoints y permite probar la autenticación Bearer.

<img width="1920" height="1267" alt="image" src="https://github.com/user-attachments/assets/9232da17-bd58-410f-8726-01503bc664cc" />


## Licencia

Este proyecto está publicado bajo la licencia **MIT**. Consulta el archivo [LICENSE](LICENSE) para el texto completo.

Puedes usar, copiar, modificar y distribuir el código con pocas condiciones (principalmente conservar el aviso de copyright y la licencia). No se ofrece garantía de ningún tipo.

Si aceptas **contribuciones** de terceros, conviene definir en el README o en `CONTRIBUTING.md` cómo enviar pull requests y bajo qué licencia aportan el código (habitualmente la misma MIT).
