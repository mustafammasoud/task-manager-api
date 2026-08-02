# Task Manager API — ASP.NET Core (C#), layered architecture

A CRUD API for managing daily tasks, built with ASP.NET Core 10 **Controllers**
 and a clean layered structure: each class has exactly one
job.

## Architecture

```
Controllers/          → HTTP layer only. Routes requests to the service,
                         maps results to status codes. No business logic,
                         no direct repository access.
Services/              → Business rules (existence checks, "title can't be
                         blank on PATCH", etc). Talks to the repository via
                         its interface. Throws domain exceptions instead of
                         deciding HTTP status codes.
Repositories/           → Persistence only. No validation, no business rules.
                         Swap InMemoryTaskRepository for an EF Core
                         implementation without touching Services/Controllers.
Models/Entities/         → Internal domain model (TaskItem, TaskState).
Models/Requests/          → One DTO per write operation:
                           CreateTaskRequest, ReplaceTaskRequest (PUT),
                           PatchTaskRequest (PATCH) — each only exposes/
                           validates the fields that operation needs.
Models/Responses/          → TaskResponse — the single shape returned to clients.
Mapping/                    → TaskMapper — the only place that converts
                           between entities and DTOs.
Exceptions/                  → TaskNotFoundException, InvalidTaskDataException —
                           domain-level errors, HTTP-agnostic.
Middleware/                   → ExceptionHandlingMiddleware — the single place
                           that maps exceptions to HTTP status codes/JSON.
Program.cs                     → Composition root only: DI registration and
                           the middleware pipeline. No logic.
```

Request flow for a typical call (e.g. `PATCH /tasks/{id}`):
`Controller` (parses route/body) → `ITaskService` (business rule: title not
blank if present) → `ITaskRepository` (fetch/update) → `TaskMapper` (entity
→ response) → back up through the layers → any thrown domain exception is
caught once, centrally, by `ExceptionHandlingMiddleware` and turned into the
right status code.

## Requirements
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup & Run

```bash
cd task-manager-api
dotnet restore
dotnet run
```

API: `http://localhost:5048` · Swagger UI: `http://localhost:5048/swagger`

## Data Model

```json
{
  "id": "guid, auto-generated",
  "title": "string, required, cannot be empty/whitespace",
  "description": "string, optional",
  "status": "\"pending\" | \"completed\" (defaults to \"pending\")",
  "createdAt": "ISO 8601 timestamp, auto-generated"
}
```

## Endpoints

| Method | Path          | Controller action | Description |
|--------|---------------|--------------------|-------------|
| POST   | `/tasks`      | `Create`           | Create a new task |
| GET    | `/tasks`      | `GetAll`           | List all tasks (optional `?status=pending\|completed`) |
| GET    | `/tasks/{id}` | `GetById`          | Get a single task by id (guid) |
| PUT    | `/tasks/{id}` | `Replace`          | Full update (title required) |
| PATCH  | `/tasks/{id}` | `Update`           | Partial update (only provided fields change) |
| DELETE | `/tasks/{id}` | `Delete`           | Delete a task |

## Validation & Error Handling
- `POST`/`PUT`: `title` required and non-blank — enforced by `[Required]` /
  `[MinLength(1)]` data annotations on `CreateTaskRequest` /
  `ReplaceTaskRequest`; `[ApiController]` auto-returns `400` with
  `ValidationProblemDetails` if violated — no manual checks needed in the controller.
- `PATCH`: `title`, if supplied, cannot be blank — this can't be expressed
  with `[Required]` (the field is optional), so `TaskService` checks it and
  throws `InvalidTaskDataException` → `400` via the middleware.
- Unknown task id on `GET`/`PUT`/`PATCH`/`DELETE` → `TaskService` throws
  `TaskNotFoundException` → `404` via the middleware, body `{ "detail": "..." }`.
- Any unhandled exception → `500` with a generic message (logged server-side).

**Known simplification:** `PATCH` treats `null` as "not supplied," so you
can't use it to explicitly clear `description` back to `null`. For that,
upgrade `PatchTaskRequest.Description` to `Optional<string?>`/JSON Patch.

## Example requests

```bash
# Create
curl -X POST http://localhost:5048/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Buy groceries", "description": "Milk, eggs, bread"}'

# List all / filtered
curl http://localhost:5048/tasks
curl "http://localhost:5048/tasks?status=completed"

# Get one
curl http://localhost:5048/tasks/<id>

# Partial update
curl -X PATCH http://localhost:5048/tasks/<id> \
  -H "Content-Type: application/json" \
  -d '{"status": "completed"}'

# Full update
curl -X PUT http://localhost:5048/tasks/<id> \
  -H "Content-Type: application/json" \
  -d '{"title": "Buy groceries", "description": "Milk, eggs", "status": "pending"}'

# Delete
curl -X DELETE http://localhost:5048/tasks/<id>
```

## Testing with Postman
1. `dotnet run`
2. Import `http://localhost:5048/swagger/v1/swagger.json` into Postman (Import > Link) to auto-generate a collection.
3. Suggested cases:
   - Create with valid title → `201`, `Location` header points at `GET /tasks/{id}`.
   - Create with missing/blank title → `400` (ValidationProblemDetails).
   - List all → `200` array; filter by `?status=completed` → only matches.
   - Get by valid id → `200`; unknown id → `404`.
   - PATCH status only → `200`, other fields unchanged; PATCH blank title → `400`.
   - PUT full payload → `200`; PUT missing title → `400`.
   - Delete → `204`; delete again → `404`.
