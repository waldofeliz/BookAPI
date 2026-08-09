# Documentación BookAPI

Documentación oficial del proyecto **BookAPI** — API REST para la gestión de recursos bibliotecarios.

## Índice

### Documentación funcional

| Documento | Descripción |
|-----------|-------------|
| [Visión general](./funcional/vision-general.md) | Propósito, alcance, actores y módulos del sistema |
| [Casos de uso](./funcional/casos-de-uso.md) | Flujos funcionales detallados por módulo |
| [Reglas de negocio](./funcional/reglas-de-negocio.md) | Validaciones, invariantes y políticas de dominio |

### Documentación técnica

| Documento | Descripción |
|-----------|-------------|
| [Arquitectura](./tecnica/arquitectura.md) | Capas, patrones, dependencias y flujo de datos |
| [Referencia API](./tecnica/referencia-api.md) | Endpoints, contratos request/response y códigos HTTP |
| [Modelo de datos](./tecnica/modelo-datos.md) | Entidades, relaciones, tablas y migraciones |
| [Configuración](./tecnica/configuracion.md) | appsettings, variables de entorno y secretos |
| [Despliegue y DevOps](./tecnica/despliegue-devops.md) | Docker, CI/CD, publicación y checklist producción |
| [Seguridad](./tecnica/seguridad.md) | Autenticación JWT, Identity, CORS y buenas prácticas |
| [Pruebas](./tecnica/pruebas.md) | Estrategia de testing, ejecución y cobertura |
| [ADR-001: Monolito modular](./tecnica/adr/001-monolito-modular.md) | Decisión de no migrar a microservicios |

## Información rápida

| Aspecto | Detalle |
|---------|---------|
| **Stack** | .NET 9, ASP.NET Core, EF Core, SQL Server |
| **Patrón** | Clean Architecture + CQRS (MediatR) |
| **Auth** | JWT + Refresh Token con rotación |
| **Swagger** | `http://localhost:5143/swagger` (solo Development) |
| **Health** | `GET /health` |

## Convenciones de rutas

Todas las rutas de la API usan el prefijo `api/v1/`. El enrutamiento es **case-insensitive** (por ejemplo, `/api/v1/auth/login` y `/api/v1/Auth/login` son equivalentes).

## Mantenimiento

Actualizar esta documentación cuando se agreguen:

- Nuevos endpoints o cambios en contratos
- Nuevas entidades o migraciones
- Cambios en configuración, Docker o CI/CD
- Nuevas reglas de negocio o validaciones
