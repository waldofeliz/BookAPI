# Seguridad

## Modelo de seguridad

BookAPI implementa un modelo de **autenticación basada en tokens** con dos niveles:

1. **Access Token (JWT):** corta duración, se envía en cada petición protegida
2. **Refresh Token:** larga duración, opaco, almacenado hasheado en BD

No hay autorización basada en roles actualmente; todos los usuarios autenticados tienen acceso completo al CRUD.

## Autenticación JWT

### Emisión

Servicio: `Infrastructure/Security/JwtTokenService.cs`

| Parámetro | Valor |
|-----------|-------|
| Algoritmo | HMAC-SHA256 |
| Claims | `sub`, `jti`, `email`, `nameidentifier`, `name` |
| Validación | Issuer, Audience, SigningKey, Lifetime |
| ClockSkew | 1 minuto |

### Uso en peticiones

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

Endpoints protegidos usan `[Authorize]`. Sin token o con token inválido → **401**.

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
| A01 Broken Access Control | ⚠️ Parcial | Auth sí; sin roles granulares |
| A02 Cryptographic Failures | ✅ Mitigado | JWT firmado, refresh hasheado |
| A03 Injection | ✅ Mitigado | EF Core parametrizado, FluentValidation |
| A04 Insecure Design | ⚠️ Parcial | Sin rate limiting en auth |
| A05 Security Misconfiguration | ✅ Mitigado | Secretos fuera de appsettings base |
| A07 Identification Failures | ✅ Mitigado | Identity + lockout + rotación |
| A09 Logging Failures | ✅ Mitigado | Serilog configurado |

## Mejoras de seguridad planificadas

1. **Rate limiting** en `/Auth/login` y `/Auth/register`
2. **Roles:** Admin, Librarian, Reader
3. **Políticas de autorización** granulares
4. **HTTPS obligatorio** en Production
5. **Auditoría de eventos de seguridad** (login fallido, refresh reutilizado)
