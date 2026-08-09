# Seguridad

## Modelo de seguridad

BookAPI implementa un modelo de **autenticación basada en tokens** con **autorización por roles**:

1. **Access Token (JWT):** corta duración, incluye roles en claims
2. **Refresh Token:** larga duración, opaco, almacenado hasheado en BD

### Roles

| Rol | Permisos |
|-----|----------|
| **Admin** | Lectura y escritura del catálogo + promover usuarios a Admin |
| **Editor** | Lectura y escritura del catálogo (rol por defecto al registrarse) |
| **Reader** | Solo lectura del catálogo |

### Políticas de autorización

| Política | Roles permitidos |
|----------|------------------|
| `CanReadCatalog` | Admin, Editor, Reader |
| `CanManageCatalog` | Admin, Editor |

Endpoints de administración (`/api/v1/Admin/*`) requieren rol **Admin**.

## Autenticación JWT

### Emisión

Servicio: `Infrastructure/Security/JwtTokenService.cs`

| Parámetro | Valor |
|-----------|-------|
| Algoritmo | HMAC-SHA256 |
| Claims | `sub`, `jti`, `email`, `nameidentifier`, `name`, `role` |
| Validación | Issuer, Audience, SigningKey, Lifetime |
| ClockSkew | 1 minuto |

### Uso en peticiones

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

Sin token o token inválido → **401**. Rol insuficiente → **403**.

## Rate limiting (auth)

Endpoints `register`, `login` y `refresh` tienen límite por IP (ventana fija, configurable en `RateLimiting:Auth`). Complementa el lockout de Identity.

| Configuración | Default |
|---------------|---------|
| `PermitLimit` | 10 |
| `WindowSeconds` | 60 |

Respuesta al exceder límite: **429 Too Many Requests**.

## Bootstrap de administradores

En `Admin:BootstrapEmails` (o variables `Admin__BootstrapEmails__N`) se pueden listar emails de usuarios **ya registrados** que se promoverán a Admin al arranque (`AdminBootstrapSeeder`).

Promoción manual: `POST /api/v1/Admin/users/promote` (solo Admin).

Asignación de rol de catálogo: `POST /api/v1/Admin/users/assign-role` con `Editor` o `Reader` (solo Admin).

## Refresh Token — Rotación segura

```mermaid
sequenceDiagram
    participant C as Cliente
    participant API as AuthService
    participant DB as RefreshTokens

    C->>API: refreshToken (texto plano)
    API->>API: SHA-256 → hash
    API->>DB: Buscar por hash
    alt Token revocado (reutilización)
        API->>DB: Revocar TODOS los tokens del usuario
        API-->>C: 401
    else Token válido
        API->>DB: Revocar token actual
        API->>DB: Crear nuevo token
        API-->>C: Nuevo access + refresh
    end
```

| Medida | Implementación |
|--------|----------------|
| Almacenamiento | Solo hash SHA-256 (`TokenHasher.cs`) |
| Rotación | Cada refresh revoca el anterior |
| Detección de reutilización | Token revocado reutilizado → revoca todos |
| Expiración | 7 días (configurable) |

## ASP.NET Core Identity

| Política | Valor |
|----------|-------|
| Longitud mínima contraseña | 12 |
| Requiere mayúscula | Sí |
| Requiere minúscula | Sí |
| Requiere dígito | Sí |
| Requiere carácter especial | Sí |
| Email único | Sí |
| Lockout tras intentos fallidos | 5 intentos → 15 min |

## Auditoría segura

`CreadoPor` y `ModificadoPor` se extraen del **email del JWT** mediante `HttpCurrentUserService`. El cliente **no puede** suplantar la identidad del auditor enviando estos campos en el body.

## CORS

| Entorno | Comportamiento |
|---------|----------------|
| Production | Solo orígenes en `Cors:AllowedOrigins` |
| Development (sin orígenes) | `AllowAnyOrigin` |
| Development (con orígenes) | Orígenes específicos |

## Manejo de errores y fuga de información

`ExceptionHandlingMiddleware`:

| Entorno | Error 500 |
|---------|-----------|
| Development | Muestra `ex.Message` |
| Production | Mensaje genérico: "Ocurrió un error interno" |

Errores se registran con Serilog incluyendo método y ruta HTTP.

## Secretos — Buenas prácticas

| ❌ No hacer | ✅ Hacer |
|------------|---------|
| Commitear `SecretKey` real | User Secrets en desarrollo |
| Password SQL en `appsettings.json` base | Variables de entorno en producción |
| Reutilizar claves entre entornos | Clave única por entorno |
| Loguear tokens completos | Loguear solo metadata (jti, userId) |

## OWASP Top 10 — Estado actual

| Riesgo | Estado | Notas |
|--------|--------|-------|
| A01 Broken Access Control | ✅ Mitigado | Roles, políticas y endpoint Admin protegido |
| A02 Cryptographic Failures | ✅ Mitigado | JWT firmado, refresh hasheado |
| A03 Injection | ✅ Mitigado | EF Core parametrizado, FluentValidation |
| A04 Insecure Design | ✅ Mitigado | Rate limiting en auth |
| A05 Security Misconfiguration | ✅ Mitigado | Secretos fuera de appsettings base |
| A07 Identification Failures | ✅ Mitigado | Identity + lockout + rotación |
| A09 Logging Failures | ✅ Mitigado | Serilog configurado |

## Mejoras de seguridad planificadas

1. **HTTPS obligatorio** en Production
2. **Auditoría de eventos de seguridad** (login fallido, refresh reutilizado)
