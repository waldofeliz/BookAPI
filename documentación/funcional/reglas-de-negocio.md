# Reglas de negocio

## Autenticación y contraseñas

| Regla | Descripción |
|-------|-------------|
| RN-AUTH-01 | El email debe ser único en el sistema |
| RN-AUTH-02 | La contraseña debe tener mínimo **12 caracteres** |
| RN-AUTH-03 | La contraseña debe incluir mayúscula, minúscula, dígito y carácter especial |
| RN-AUTH-04 | Tras **5 intentos fallidos** de login, la cuenta se bloquea **15 minutos** |
| RN-AUTH-05 | El access token expira en **15 minutos** (configurable) |
| RN-AUTH-06 | El refresh token expira en **7 días** (configurable) |
| RN-AUTH-07 | Al renovar sesión, el refresh token anterior se revoca (rotación) |
| RN-AUTH-08 | Si se reutiliza un refresh token ya revocado, se revocan **todos** los tokens del usuario |

## Libros

| Regla | Descripción |
|-------|-------------|
| RN-LIB-01 | El título es obligatorio (máx. 200 caracteres en dominio, 250 en validación API) |
| RN-LIB-02 | El ISBN es obligatorio, longitud entre **10 y 17** caracteres |
| RN-LIB-03 | El ISBN debe ser **único** en todo el catálogo |
| RN-LIB-04 | Las páginas deben ser mayor a **0** |
| RN-LIB-05 | La URL de portada, si se envía, debe ser una URI absoluta válida |
| RN-LIB-06 | El subtítulo, si se envía, no puede exceder **250** caracteres |
| RN-LIB-07 | La editora referenciada (`editoraId`) debe existir en el sistema |
| RN-LIB-08 | Todos los autores en `autorIds` deben existir en el sistema |
| RN-LIB-09 | Los autores se asignan con un **orden** (1 = primer autor, 2 = segundo, etc.) |
| RN-LIB-10 | Al actualizar autores, la lista enviada **reemplaza** la asignación anterior |
| RN-LIB-11 | `creadoPor` y `modificadoPor` se obtienen del **email del JWT**, no del body del cliente |

## Autores

| Regla | Descripción |
|-------|-------------|
| RN-AUT-01 | Nombre y apellido son obligatorios (máx. 50 caracteres cada uno) |
| RN-AUT-02 | La combinación nombre + apellido debe ser **única** |
| RN-AUT-03 | No se puede eliminar un autor vinculado a libros (restricción FK) |
| RN-AUT-04 | La biografía no puede exceder **2000** caracteres |
| RN-AUT-05 | La nacionalidad no puede exceder **100** caracteres |

## Editoras

| Regla | Descripción |
|-------|-------------|
| RN-EDI-01 | El nombre es obligatorio (máx. 50 caracteres) |
| RN-EDI-02 | El nombre de editora debe ser **único** |
| RN-EDI-03 | El sitio web, si se envía, debe ser URI absoluta (máx. 500 caracteres) |
| RN-EDI-04 | El teléfono no puede exceder **20** caracteres |
| RN-EDI-05 | El país no puede exceder **50** caracteres |
| RN-EDI-06 | Al eliminar una editora, los libros asociados quedan sin editora (`EditoraId = null`) |

## Paginación y búsqueda

| Regla | Descripción |
|-------|-------------|
| RN-PAG-01 | `page` mínimo: **1** (valores menores se normalizan a 1) |
| RN-PAG-02 | `pageSize` rango: **1–100** (fuera de rango se usa 10 por defecto) |
| RN-PAG-03 | `search` máximo **100** caracteres |
| RN-PAG-04 | La respuesta incluye `meta.totalCount` para calcular páginas totales |

## Auditoría

| Campo | Origen | Descripción |
|-------|--------|-------------|
| `creadoPor` | Email del JWT al crear | No editable por el cliente |
| `creadoEn` | `DateTime.UtcNow` al crear | Timestamp UTC |
| `modificadoPor` | Email del JWT al actualizar | No editable por el cliente |
| `modificadoEn` | `DateTime.UtcNow` al actualizar | Timestamp UTC |
| `version` | Incremento automático | Token de concurrencia optimista |

## Estados

Los recursos Libro, Autor y Editora tienen un campo `estado` (activo/inactivo). En las operaciones de actualización se puede modificar. Los DTOs de **Libro** y **Autor** no exponen `estado` en la respuesta actual; **Editora** sí lo incluye.
