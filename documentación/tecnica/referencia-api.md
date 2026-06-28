# Referencia API

**Base URL (local):** `http://localhost:5143` o `https://localhost:7101`  
**Prefijo:** `/api/v1/`  
**Formato:** JSON (camelCase)  
**Autenticación:** `Authorization: Bearer <accessToken>`

---

## Health checks

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| GET | `/health` | No | Estado general (incluye DB) |
| GET | `/health/ready` | No | Readiness probe |

---

## Autenticación

Todos los endpoints son **públicos** (`[AllowAnonymous]`).

### POST `/api/v1/Auth/register`

**Request:**
```json
{
  "email": "usuario@ejemplo.com",
  "password": "MiPassword123!"
}
```

**Response 200:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "base64...",
  "accessTokenExpiresAtUtc": "2026-06-28T13:30:00Z"
}
```

### POST `/api/v1/Auth/login`

Mismo contrato que register.

### POST `/api/v1/Auth/refresh`

**Request:**
```json
{
  "refreshToken": "base64..."
}
```

**Response 200:** Igual que login.

---

## Libros

Requieren **JWT** (`[Authorize]`).

### POST `/api/v1/Libros`

**Request:**
```json
{
  "titulo": "Clean Architecture",
  "isbn": "9780134494166",
  "publicadoEn": "2017-09-13T00:00:00Z",
  "descripcion": "Guía de arquitectura de software",
  "subTitulo": "A Craftsman's Guide",
  "coverImageUrl": "https://example.com/cover.jpg",
  "edicion": "1ra",
  "paginas": 432,
  "lenguaje": "en",
  "editoraId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "autorIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    "3fa85f64-5717-4562-b3fc-2c963f66afa8"
  ]
}
```

**Response 201:** `LibroDto`

### GET `/api/v1/Libros`

**Query params:**

| Parámetro | Tipo | Default | Descripción |
|-----------|------|---------|-------------|
| `page` | int | 1 | Número de página |
| `pageSize` | int | 10 | Elementos por página (máx. 100) |
| `search` | string | null | Busca en título, ISBN y nombre de editora |

**Response 200:**
```json
{
  "items": [ /* LibroDto[] */ ],
  "meta": {
    "page": 1,
    "pageSize": 10,
    "totalCount": 42
  }
}
```

### GET `/api/v1/Libros/{id}`

**Response 200:** `LibroDto` | **404** si no existe

### PUT `/api/v1/Libros/{id}`

**Request:** Igual que crear + `"estado": true`

**Response 200:** `LibroDto`

### DELETE `/api/v1/Libros/{id}`

**Response 204** sin body

### LibroDto

```json
{
  "id": "guid",
  "titulo": "string",
  "isbn": "string",
  "publicadoEn": "datetime|null",
  "descripcion": "string|null",
  "coverImageUrl": "string|null",
  "lenguaje": "string|null",
  "paginas": 432,
  "edicion": "string|null",
  "subTitulo": "string|null",
  "editoraId": "guid|null",
  "editoraNombre": "string|null",
  "autores": [
    {
      "id": "guid",
      "nombre": "Robert",
      "apellido": "Martin",
      "orden": 1
    }
  ]
}
```

---

## Autores

Requieren **JWT**.

| Método | Ruta | Body | Response |
|--------|------|------|----------|
| POST | `/api/v1/Autores` | CreateAutorRequest | 201 AutorDto |
| GET | `/api/v1/Autores` | — | 200 PagedResult\<AutorDto\> |
| GET | `/api/v1/Autores/{id}` | — | 200 / 404 |
| PUT | `/api/v1/Autores/{id}` | UpdateAutorRequest | 200 AutorDto |
| DELETE | `/api/v1/Autores/{id}` | — | 204 |

**CreateAutorRequest:**
```json
{
  "nombre": "Gabriel",
  "apellido": "García Márquez",
  "cumpleanio": "1927-03-06T00:00:00Z",
  "biografia": "Escritor colombiano...",
  "nacionalidad": "Colombia"
}
```

**UpdateAutorRequest:** Igual + `"estado": true`

**AutorDto:**
```json
{
  "id": "guid",
  "nombre": "string",
  "apellido": "string",
  "cumpleanio": "datetime",
  "biografia": "string|null",
  "nacionalidad": "string|null"
}
```

---

## Editoras

Requieren **JWT**.

| Método | Ruta | Body | Response |
|--------|------|------|----------|
| POST | `/api/v1/Editoras` | CreateEditoraRequest | 201 EditoraDto |
| GET | `/api/v1/Editoras` | — | 200 PagedResult\<EditoraDto\> |
| GET | `/api/v1/Editoras/{id}` | — | 200 / 404 |
| PUT | `/api/v1/Editoras/{id}` | UpdateEditoraRequest | 200 EditoraDto |
| DELETE | `/api/v1/Editoras/{id}` | — | 204 |

**CreateEditoraRequest:**
```json
{
  "nombre": "Penguin Books",
  "descripcion": "Casa editorial británica",
  "direccion": "London, UK",
  "pais": "Reino Unido",
  "website": "https://www.penguin.co.uk",
  "telefono": "+44-20-1234-5678"
}
```

**EditoraDto:**
```json
{
  "id": "guid",
  "nombre": "string",
  "descripcion": "string|null",
  "direccion": "string|null",
  "pais": "string|null",
  "website": "string|null",
  "telefono": "string|null",
  "estado": true
}
```

---

## Códigos de respuesta HTTP

| Código | Significado | Cuándo |
|--------|-------------|--------|
| 200 | OK | Operación exitosa |
| 201 | Created | Recurso creado (POST) |
| 204 | No Content | Eliminación exitosa |
| 400 | Bad Request | Validación fallida |
| 401 | Unauthorized | Token ausente/inválido o credenciales incorrectas |
| 404 | Not Found | Recurso no encontrado |
| 409 | Conflict | Duplicado (ISBN, nombre) |
| 500 | Internal Server Error | Error no controlado |

**Formato de error (ProblemDetails):**
```json
{
  "title": "Validation error",
  "status": 400,
  "detail": "El título es requerido."
}
```

---

## Ejemplo: flujo completo con cURL

```bash
# 1. Registro
curl -X POST http://localhost:5143/api/v1/Auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"SecurePass123!"}'

# 2. Crear editora
curl -X POST http://localhost:5143/api/v1/Editoras \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"nombre":"OReilly Media"}'

# 3. Crear autor
curl -X POST http://localhost:5143/api/v1/Autores \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"nombre":"Martin","apellido":"Fowler","cumpleanio":"1963-12-18"}'

# 4. Crear libro con relaciones
curl -X POST http://localhost:5143/api/v1/Libros \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo":"Refactoring",
    "isbn":"9780134757599",
    "publicadoEn":"2018-11-19",
    "paginas":448,
    "editoraId":"<editora-guid>",
    "autorIds":["<autor-guid>"]
  }'
```
